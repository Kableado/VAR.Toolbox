using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using VAR.ScreenAutomation.Code;
using VAR.ScreenAutomation.Interfaces;

namespace VAR.ScreenAutomation.Bots
{
    public class TetrisBot : IAutomationBot
    {
        private TetrisGrid _grid;

        private List<TetrisShape> _currentShape;
        private TetrisGrid _workGrid;

        private bool _shapeFound = false;
        private int _shapeX;
        private int _shapeY;
        private double _bestEvaluation = double.MinValue;
        private int _bestXOffset = 0;
        private int _bestRotation = 0;

        public string Name => "Tetris";

        private const int DefaultGridWidth = 10;
        private const int DefaultGridHeight = 20;

        public IConfiguration GetDefaultConfiguration()
        {
            var defaultConfiguration = new MemoryBackedConfiguration();
            defaultConfiguration.Set("GridWidth", DefaultGridWidth);
            defaultConfiguration.Set("GridHeight", DefaultGridHeight);
            return defaultConfiguration;
        }

        public void Init(IOutputHandler output, IConfiguration config)
        {
            int gridWidth = config.Get("GridWidth", DefaultGridWidth);
            int gridHeight = config.Get("GridHeight", DefaultGridHeight);

            _grid = new TetrisGrid(gridWidth, gridHeight);
            _workGrid = new TetrisGrid(gridWidth, gridHeight);
            _currentShape = new List<TetrisShape>
            {
                new TetrisShape(),
                new TetrisShape(),
                new TetrisShape(),
                new TetrisShape(),
            };
            output.Clean();
        }

        public Bitmap Process(Bitmap bmpInput, IOutputHandler output)
        {
            // Initialize grid
            _grid.SampleFromBitmap(bmpInput);
            _grid.MarkGround();

            // Identify current tetronino
            _shapeFound = false;
            if (_grid.SearchFirstCell(1, out _shapeX, out _shapeY))
            {
                _currentShape[0].SampleFromGrid(_grid, _shapeX, _shapeY, 1);
                _shapeFound = _currentShape[0].IsValid();
                for (int i = 1; i < 4; i++)
                {
                    _currentShape[i].RotateCW(_currentShape[i - 1]);
                }
            }

            // Search best action
            _bestEvaluation = double.MinValue;
            _bestXOffset = 0;
            _bestRotation = 0;
            if (_shapeFound)
            {
                _workGrid.SampleOther(_grid, 2, 2);
                if (_currentShape[0].Drop(_workGrid, _shapeX, _shapeY, 1))
                {
                    _bestXOffset = 0;
                    _bestRotation = 0;
                    _bestEvaluation = EvaluateWorkingGrid();
                }

                int offsetX = 1;
                double newEvaluation;

                for (int rotation = 0; rotation < 4; rotation++)
                {
                    // Check positive offset
                    offsetX = 1;
                    do
                    {
                        _workGrid.SampleOther(_grid, 2, 2);
                        if (_currentShape[rotation].Drop(_workGrid, _shapeX + offsetX, _shapeY, 1))
                        {
                            newEvaluation = EvaluateWorkingGrid();
                            if (newEvaluation > _bestEvaluation)
                            {
                                _bestEvaluation = newEvaluation;
                                _bestXOffset = offsetX;
                                _bestRotation = rotation;
                            }
                        }
                        else
                        {
                            break;
                        }
                        offsetX++;
                    } while (true);

                    // Check negative offset
                    offsetX = -1;
                    do
                    {
                        _workGrid.SampleOther(_grid, 2, 2);
                        if (_currentShape[rotation].Drop(_workGrid, _shapeX + offsetX, _shapeY, 1))
                        {
                            newEvaluation = EvaluateWorkingGrid();
                            if (newEvaluation > _bestEvaluation)
                            {
                                _bestEvaluation = newEvaluation;
                                _bestXOffset = offsetX;
                                _bestRotation = rotation;
                            }
                        }
                        else
                        {
                            break;
                        }
                        offsetX--;
                    } while (true);
                }
            }
            else
            {
                _workGrid.SampleOther(_grid, 2, 2);
            }

            // DEBUG Show information
            _workGrid.SampleOther(_grid, 2, 2);
            if (_shapeFound)
            {
                _currentShape[_bestRotation].Drop(_workGrid, _shapeX + _bestXOffset, _shapeY, 1);
            }
            _workGrid.Draw(bmpInput);

            return bmpInput;
        }

        private double EvaluateWorkingGrid()
        {
            return _workGrid.Evaluate(
                aggregateHeightWeight: -0.6,
                completeLinesWeight: 0.8,
                holesWeight: -0.5,
                bumpinessWeight: -0.2,
                maxHeightWeight: -0.25);
        }

        private bool _canShot = true;
        private int _lastShotShapeY = 0;
        private DateTime _lastShotDateTime;

        public string ResponseKeys()
        {
            if (_shapeFound == false) { return string.Empty; }

            if (_canShot == false && (_shapeY < _lastShotShapeY || _lastShotDateTime.AddMilliseconds(500) < DateTime.UtcNow))
            {
                _canShot = true;
            }

            if (_bestRotation == 0 && _bestXOffset == 0 && _bestEvaluation > double.MinValue)
            {
                if (_canShot)
                {
                    _canShot = false;
                    _lastShotShapeY = _shapeY;
                    _lastShotDateTime = DateTime.UtcNow;
                    return " ";
                }
            }

            if (_bestRotation != 0 && _bestXOffset < 0) { return "{UP}{LEFT}"; }
            if (_bestRotation != 0 && _bestXOffset > 0) { return "{UP}{RIGHT}"; }
            if (_bestRotation != 0) { return "{UP}"; }
            if (_bestXOffset < 0) { return "{LEFT}"; }
            if (_bestXOffset > 0) { return "{RIGHT}"; }

            return string.Empty;
        }
    }

    public class TetrisShape
    {
        public const int ShapeSize = 4;

        private byte[][] _cells = null;

        private int _count = 0;

        public TetrisShape(byte[][] cells = null)
        {
            _cells = new byte[ShapeSize][];
            for (int y = 0; y < ShapeSize; y++)
            {
                _cells[y] = new byte[ShapeSize];
            }

            _count = 0;
            if (cells != null)
            {
                for (int j = 0; j < ShapeSize; j++)
                {
                    if (j >= cells.Length) { break; }
                    for (int i = 0; i < ShapeSize; i++)
                    {
                        if (i >= cells[j].Length) { break; }
                        _cells[j][i] = cells[j][i];
                        if (_cells[j][i] != 0) { _count++; }
                    }
                }
            }
        }

        public int GetCount()
        {
            return _count;
        }

        private static List<TetrisShape> _defaultShapes = null;

        public bool IsValid()
        {
            if (_defaultShapes == null)
            {
                _defaultShapes = new List<TetrisShape>
                {
                    // I
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, 1, 1, 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, },
                        new byte[]{ 1, },
                        new byte[]{ 1, },
                        new byte[]{ 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, },
                        new byte[]{ 1, },
                        new byte[]{ 1, },
                        new byte[]{ 1, },
                    }),

                    // J
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, },
                        new byte[]{ 1, 1, 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, 1, },
                        new byte[]{ 1, },
                        new byte[]{ 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, 1, 1, },
                        new byte[]{ 0, 0, 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 0, 1, },
                        new byte[]{ 0, 1, },
                        new byte[]{ 1, 1, },
                    }),

                    // L
                    new TetrisShape(new byte[][]{
                        new byte[]{ 0, 0, 1, },
                        new byte[]{ 1, 1, 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, },
                        new byte[]{ 1, },
                        new byte[]{ 1, 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, 1, 1, },
                        new byte[]{ 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, 1, },
                        new byte[]{ 0, 1, },
                        new byte[]{ 0, 1, },
                    }),

                    // S
                    new TetrisShape(new byte[][]{
                        new byte[]{ 0, 1, 1, },
                        new byte[]{ 1, 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, },
                        new byte[]{ 1, 1, },
                        new byte[]{ 0, 1, },
                    }),

                    // T
                    new TetrisShape(new byte[][]{
                        new byte[]{ 0, 1, },
                        new byte[]{ 1, 1, 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, },
                        new byte[]{ 1, 1, },
                        new byte[]{ 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, 1, 1, },
                        new byte[]{ 0, 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 0, 1, },
                        new byte[]{ 1, 1, },
                        new byte[]{ 0, 1, },
                    }),

                    // Z
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, 1, },
                        new byte[]{ 0, 1, 1, },
                    }),
                    new TetrisShape(new byte[][]{
                        new byte[]{ 0, 1, },
                        new byte[]{ 1, 1, },
                        new byte[]{ 1, },
                    }),

                    // O
                    new TetrisShape(new byte[][]{
                        new byte[]{ 1, 1, },
                        new byte[]{ 1, 1, },
                    })
                };
            }

            if (_count != 4) { return false; }
            bool matchesAnyDefault = _defaultShapes.Any(ts => CompareShape(ts));
            return matchesAnyDefault;
        }

        public void Copy(TetrisShape shape)
        {
            for (int j = 0; j < ShapeSize; j++)
            {
                for (int i = 0; i < ShapeSize; i++)
                {
                    _cells[j][i] = shape._cells[j][i];
                    _count = shape._count;
                }
            }
        }

        public bool CompareShape(TetrisShape shape)
        {
            for (int j = 0; j < ShapeSize; j++)
            {
                for (int i = 0; i < ShapeSize; i++)
                {
                    if (_cells[j][i] != shape._cells[j][i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void SampleFromGrid(TetrisGrid grid, int x, int y, byte value)
        {
            _count = 0;
            for (int j = 0; j < ShapeSize; j++)
            {
                for (int i = 0; i < ShapeSize; i++)
                {
                    if (grid.Get(x + i, y + j) == value)
                    {
                        _cells[j][i] = 1;
                        _count++;
                    }
                    else
                    {
                        _cells[j][i] = 0;
                    }
                }
            }
        }

        public void PutOnGrid(TetrisGrid grid, int x, int y, byte value)
        {
            for (int j = 0; j < ShapeSize; j++)
            {
                for (int i = 0; i < ShapeSize; i++)
                {
                    if (_cells[j][i] == 0) { continue; }
                    grid.Set(x + i, y + j, value);
                }
            }
        }

        public bool CheckIntersection(TetrisGrid grid, int x, int y)
        {
            for (int j = 0; j < ShapeSize; j++)
            {
                for (int i = 0; i < ShapeSize; i++)
                {
                    if (_cells[j][i] == 0) { continue; }
                    if (grid.Get(x + i, y + j) != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Drop(TetrisGrid grid, int x, int y, byte value)
        {
            if (CheckIntersection(grid, x, y)) { return false; }
            while (CheckIntersection(grid, x, y + 1) == false)
            {
                y++;
            }
            PutOnGrid(grid, x, y, value);
            return true;
        }

        public bool SearchFirstCell(byte value, out int x, out int y)
        {
            x = -1;
            y = -1;
            for (int j = 0; j < ShapeSize && y == -1; j++)
            {
                for (int i = 0; i < ShapeSize && y == -1; i++)
                {
                    if (_cells[j][i] == value)
                    {
                        y = j;
                    }
                }
            }
            if (y == -1) { return false; }
            for (int i = 0; i < ShapeSize && x == -1; i++)
            {
                for (int j = 0; j < ShapeSize && x == -1; j++)
                {
                    if (_cells[j][i] == value)
                    {
                        x = i;
                    }
                }
            }
            if (x == -1) { return false; }
            return true;
        }

        public void Offset(int x, int y)
        {
            _count = 0;
            for (int j = 0; j < ShapeSize; j++)
            {
                for (int i = 0; i < ShapeSize; i++)
                {
                    if ((j + y) < ShapeSize && (i + x) < ShapeSize)
                    {
                        _cells[j][i] = _cells[j + y][i + x];
                        if (_cells[j][i] != 0)
                        {
                            _count++;
                        }
                    }
                    else
                    {
                        _cells[j][i] = 0;
                    }
                }
            }
        }

        public void RotateCW(TetrisShape shape)
        {
            for (int j = 0; j < ShapeSize; j++)
            {
                for (int i = 0; i < ShapeSize; i++)
                {
                    _cells[i][ShapeSize - (j + 1)] = shape._cells[j][i];
                }
            }
            _count = shape._count;

            if (SearchFirstCell(1, out int offsetX, out int offsetY))
            {
                Offset(offsetX, offsetY);
            }
        }

        public void Print(IOutputHandler output)
        {
            for (int y = 0; y < ShapeSize; y++)
            {
                StringBuilder sbLine = new StringBuilder();
                for (int x = 0; x < ShapeSize; x++)
                {
                    if (_cells[y][x] == 0)
                    {
                        sbLine.Append("..");
                    }
                    else
                    {
                        sbLine.Append("[]");
                    }
                }
                output.AddLine(sbLine.ToString());
            }
        }
    }

    public class TetrisGrid
    {
        private int _gridWidth;
        private int _gridHeight;

        private byte[][] _grid = null;

        private int[] _heights = null;

        public TetrisGrid(int gridWidth, int gridHeight)
        {
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
            _grid = new byte[_gridHeight][];
            for (int y = 0; y < _gridHeight; y++)
            {
                _grid[y] = new byte[_gridWidth];
            }
            _heights = new int[_gridWidth];
        }

        public byte Get(int x, int y)
        {
            if (x >= _gridWidth || x < 0) { return 0xFF; }
            if (y >= _gridHeight || y < 0) { return 0xFF; }
            return _grid[y][x];
        }

        public void Set(int x, int y, byte value)
        {
            if (x >= _gridWidth || x < 0) { return; }
            if (y >= _gridHeight || y < 0) { return; }
            _grid[y][x] = value;
        }

        public void SampleFromBitmap(Bitmap bmp)
        {
            float xStep = bmp.Width / _gridWidth;
            float yStep = bmp.Height / _gridHeight;
            float xOff0 = xStep / 2;
            float xOff1 = xOff0 / 2;
            float xOff2 = xOff0 + xOff1;
            float yOff0 = yStep / 2;
            float yOff1 = yOff0 / 2;
            float yOff2 = yOff0 + yOff1;
            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    Color color = bmp.GetPixel(
                        x: (int)((x * xStep) + xOff0),
                        y: (int)((y * yStep) + yOff0));
                    if (color.R > 128 || color.G > 128 || color.B > 128)
                    {
                        Color color0 = bmp.GetPixel(
                            x: (int)((x * xStep) + xOff1),
                            y: (int)((y * yStep) + yOff1));
                        Color color1 = bmp.GetPixel(
                            x: (int)((x * xStep) + xOff1),
                            y: (int)((y * yStep) + yOff2));
                        Color color2 = bmp.GetPixel(
                            x: (int)((x * xStep) + xOff2),
                            y: (int)((y * yStep) + yOff1));
                        Color color3 = bmp.GetPixel(
                            x: (int)((x * xStep) + xOff2),
                            y: (int)((y * yStep) + yOff2));
                        if (
                            (color0.R > 128 || color0.G > 128 || color0.B > 128) &&
                            (color1.R > 128 || color1.G > 128 || color1.B > 128) &&
                            (color2.R > 128 || color2.G > 128 || color2.B > 128) &&
                            (color3.R > 128 || color3.G > 128 || color3.B > 128) &&
                            true)
                        {
                            _grid[y][x] = 1;
                        }
                        else
                        {
                            _grid[y][x] = 0;
                        }
                    }
                    else
                    {
                        _grid[y][x] = 0;
                    }
                }
            }
        }

        public void MarkGround()
        {
            for (int i = 0; i < _gridWidth; i++)
            {
                if (_grid[_gridHeight - 1][i] == 1)
                {
                    FloodFill(i, _gridHeight - 1, 1, 2);
                }
            }
        }

        public void FloodFill(int x, int y, byte expectedValue, byte fillValue)
        {
            if (x >= _gridWidth || x < 0) { return; }
            if (y >= _gridHeight || y < 0) { return; }
            if (_grid[y][x] != expectedValue) { return; }
            _grid[y][x] = fillValue;
            FloodFill(x - 1, y - 1, expectedValue, fillValue);
            FloodFill(x - 1, y + 0, expectedValue, fillValue);
            FloodFill(x - 1, y + 1, expectedValue, fillValue);
            FloodFill(x + 0, y - 1, expectedValue, fillValue);
            FloodFill(x + 0, y + 1, expectedValue, fillValue);
            FloodFill(x + 1, y - 1, expectedValue, fillValue);
            FloodFill(x + 1, y + 0, expectedValue, fillValue);
            FloodFill(x + 1, y + 1, expectedValue, fillValue);
        }

        public void SampleOther(TetrisGrid grid, byte value, byte setValue = 1)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    if (grid._grid[y][x] == value)
                    {
                        _grid[y][x] = setValue;
                    }
                    else
                    {
                        _grid[y][x] = 0;
                    }
                }
            }
        }

        public bool SearchFirstCell(byte value, out int x, out int y)
        {
            x = -1;
            y = -1;
            for (int j = 0; j < _gridHeight && y == -1; j++)
            {
                for (int i = 0; i < _gridWidth && y == -1; i++)
                {
                    if (_grid[j][i] == value)
                    {
                        y = j;
                    }
                }
            }
            if (y == -1) { return false; }
            for (int i = 0; i < _gridWidth && x == -1; i++)
            {
                for (int j = 0; j < _gridHeight && x == -1; j++)
                {
                    if (_grid[j][i] == value)
                    {
                        x = i;
                    }
                }
            }
            if (x == -1) { return false; }
            return true;
        }

        public bool IsCompleteLine(int y)
        {
            bool complete = true;
            for (int x = 0; x < _gridWidth; x++)
            {
                if (_grid[y][x] == 0)
                {
                    complete = false;
                    break;
                }
            }
            return complete;
        }

        public double Evaluate(double aggregateHeightWeight, double completeLinesWeight, double holesWeight, double bumpinessWeight, double maxHeightWeight)
        {
            // Calculte aggregate height
            for (int i = 0; i < _gridWidth; i++)
            {
                int j = 0;
                while (j < _gridHeight && _grid[j][i] == 0) { j++; }
                _heights[i] = _gridHeight - j;
            }
            double agregateHeight = _heights.Sum();

            // Calculate complete lines
            int completeLines = 0;
            for (int y = 0; y < _gridHeight; y++)
            {
                if (IsCompleteLine(y)) { completeLines++; }
            }

            // Calculate holes
            int holes = 0;
            for (int x = 0; x < _gridWidth; x++)
            {
                bool block = false;
                for (int y = 1; y < _gridHeight; y++)
                {
                    if (_grid[y][x] != 0 && IsCompleteLine(y) == false)
                    {
                        block = true;
                    }
                    else if (_grid[y][x] == 0 && block)
                    {
                        holes++;
                    }
                }
            }

            // Calculate bumpiness
            int bumpines = 0;
            for (int i = 1; i < _gridWidth; i++)
            {
                bumpines += Math.Abs(_heights[i] - _heights[i - 1]);
            }

            // Calculate max height
            int maxHeight = _heights.Max();

            // Evaluate formula
            double evaluationValue =
                aggregateHeightWeight * agregateHeight +
                completeLinesWeight * completeLines +
                holesWeight * holes +
                bumpinessWeight * bumpines +
                maxHeightWeight * maxHeight +
                0;
            return evaluationValue;
        }

        public void Print(IOutputHandler output)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                StringBuilder sbLine = new StringBuilder();
                for (int x = 0; x < _gridWidth; x++)
                {
                    if (_grid[y][x] == 0)
                    {
                        sbLine.Append("..");
                    }
                    else if (_grid[y][x] == 1)
                    {
                        sbLine.Append("$$");
                    }
                    else
                    {
                        sbLine.Append("[]");
                    }
                }
                output.AddLine(sbLine.ToString());
            }
        }

        public void Draw(Bitmap bmp)
        {
            float xStep = bmp.Width / (float)_gridWidth;
            float yStep = bmp.Height / (float)_gridHeight;
            float halfXStep = xStep / 2;
            float halfYStep = yStep / 2;
            float offX = halfXStep / 2;
            float offY = halfYStep / 2;

            using (Pen borderPen = new Pen(Color.DarkGray))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    for (int x = 0; x < _gridWidth; x++)
                    {
                        Brush br;
                        if (_grid[y][x] == 0)
                        {
                            br = Brushes.Black;
                        }
                        else if (_grid[y][x] == 1)
                        {
                            br = Brushes.Red;
                        }
                        else
                        {
                            br = Brushes.Blue;
                        }
                        if (br == null) { continue; }

                        g.DrawRectangle(borderPen, (xStep * x) + offX - 1, (yStep * y) + offY - 1, halfXStep + 2, halfYStep + 2);
                        g.FillRectangle(br, (xStep * x) + offX, (yStep * y) + offY, halfXStep, halfYStep);
                    }
                }
            }
        }

    }
}
