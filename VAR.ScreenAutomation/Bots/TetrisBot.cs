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
        private TetrisGrid _workGrid0;
        private TetrisGrid _workGrid1;
        private double[] _columnEvaluation;

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
            _workGrid0 = new TetrisGrid(gridWidth, gridHeight);
            _workGrid1 = new TetrisGrid(gridWidth, gridHeight);
            _columnEvaluation = new double[gridWidth];
            _currentShape = new List<TetrisShape>
            {
                new TetrisShape(),
                new TetrisShape(),
                new TetrisShape(),
                new TetrisShape(),
            };
            output.Clean();
            output.AddLine(string.Format("TetrisBot: Starting {0}", DateTime.UtcNow.ToString("s")));
        }

        public Bitmap Process(Bitmap bmpInput, IOutputHandler output)
        {
            _grid.SampleFromBitmap(bmpInput);
            SearchShape();
            SearchBestAction();

            // Show information
            _workGrid0.SampleOther(_grid, TetrisGrid.Solid, TetrisGrid.Solid);
            if (_shapeFound)
            {
                _currentShape[0].PutOnGrid(_workGrid0, _shapeX, _shapeY, TetrisGrid.Empty);
                _currentShape[_bestRotation].Drop(_workGrid0, _shapeX + _bestXOffset, _shapeY, TetrisGrid.ShapeA);
                _currentShape[0].PutOnGrid(_workGrid0, _shapeX, _shapeY, TetrisGrid.ShapeB);
            }
            _workGrid0.Draw(bmpInput, 0.75f);
            _workGrid0.SampleOther(_grid, TetrisGrid.Solid, TetrisGrid.Solid);
            _workGrid0.RemoveGround();
            _workGrid0.Draw(bmpInput, 0.25f);

            return bmpInput;
        }

        private void SearchShape()
        {
            _workGrid0.SampleOther(_grid, TetrisGrid.Solid, TetrisGrid.Solid);
            _workGrid0.RemoveGround();
            _shapeFound = false;
            for (int y = 0; y < _grid.Height; y++)
            {
                for (int x = 0; x < _grid.Width; x++)
                {
                    TetrisShape matchedShape = TetrisShape.DefaultShapes.FirstOrDefault(s => s.MatchOnGrid(_workGrid0, x, y));
                    if (matchedShape != null)
                    {
                        _workGrid1.SampleOther(_workGrid0, TetrisGrid.Solid, TetrisGrid.Solid);
                        matchedShape.PutOnGrid(_workGrid1, x, y, TetrisGrid.Empty);
                        if (matchedShape.CheckIntersection(_workGrid1, x, y + 1)) { continue; }

                        // Shape found
                        _currentShape[0].Copy(matchedShape);
                        for (int i = 1; i < 4; i++)
                        {
                            _currentShape[i].RotateCW(_currentShape[i - 1]);
                        }
                        _shapeX = x;
                        _shapeY = y;
                        _shapeFound = true;
                        break;
                    }
                }
                if (_shapeFound) { break; }
            }
        }

        private void SearchBestAction()
        {
            _bestEvaluation = double.MinValue;
            _bestXOffset = 0;
            _bestRotation = 0;
            if (!_shapeFound)
            {
                _workGrid0.SampleOther(_grid, TetrisGrid.Solid, TetrisGrid.Solid);
                return;
            }


            for (int rotation = 0; rotation < 4; rotation++)
            {
                for (int x = 0; x < _grid.Width; x++)
                {
                    _workGrid0.SampleOther(_grid, TetrisGrid.Solid, TetrisGrid.Solid);
                    _currentShape[0].PutOnGrid(_workGrid0, _shapeX, _shapeY, TetrisGrid.Empty);

                    if (_currentShape[rotation].Drop(_workGrid0, x, _shapeY, TetrisGrid.ShapeA))
                    {
                        double newEvaluation = EvaluateWorkingGrid();
                        _columnEvaluation[x] = newEvaluation;
                    }
                    else
                    {
                        _columnEvaluation[x] = double.MinValue;
                    }
                }

                // Search valid X range
                int minX = _shapeX;
                while (minX >= 0 && _columnEvaluation[minX] > double.MinValue) { minX--; }
                minX++;
                int maxX = _shapeX;
                while (maxX < _grid.Width && _columnEvaluation[maxX] > double.MinValue) { maxX++; }
                maxX--;

                // Apply best value inside valid X range
                for (int x = minX; x <= maxX; x++)
                {
                    if (_columnEvaluation[x] > _bestEvaluation)
                    {
                        _bestEvaluation = _columnEvaluation[x];
                        _bestXOffset = x - _shapeX;
                        _bestRotation = rotation;
                    }
                }
            }
        }

        private double EvaluateWorkingGrid()
        {
            return _workGrid0.Evaluate(
                aggregateHeightWeight: -0.510066,
                completeLinesWeight: 0.760666,
                holesWeight: -0.35663,
                bumpinessWeight: -0.184483);
        }

        private const int ShotCooldownFrames = 2;
        private int _shotCooldown = ShotCooldownFrames;

        public string ResponseKeys()
        {
            if (_shapeFound == false) { return string.Empty; }

            if (_bestRotation == 0 && _bestXOffset == 0 && _bestEvaluation > double.MinValue)
            {
                if (_shotCooldown <= 0)
                {
                    _shotCooldown = ShotCooldownFrames;
                    return " ";
                }
                else
                {
                    _shotCooldown--;
                    return string.Empty;
                }
            }

            _shotCooldown = ShotCooldownFrames;

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

        public static List<TetrisShape> DefaultShapes
        {
            get
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
                return _defaultShapes;
            }
        }

        public bool IsValid()
        {
            if (_count != 4) { return false; }
            bool matchesAnyDefault = DefaultShapes.Any(ts => CompareShape(ts));
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

        public bool MatchOnGrid(TetrisGrid grid, int x, int y)
        {
            for (int j = 0; j < ShapeSize; j++)
            {
                for (int i = 0; i < ShapeSize; i++)
                {
                    int currentCell = grid.Get(x + i, y + j);
                    if (_cells[j][i] == 0)
                    {
                        if (currentCell != 0 && currentCell != 0xFF)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (currentCell == 0 || currentCell == 0xFF)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }

    public class TetrisGrid
    {
        public const byte Empty = 0;
        public const byte Solid = 1;
        public const byte ShapeA = 2;
        public const byte ShapeB = 3;

        private int _gridWidth;
        private int _gridHeight;

        public int Width { get { return _gridWidth; } }
        public int Height { get { return _gridHeight; } }

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

        public void RemoveGround()
        {
            for (int i = 0; i < _gridWidth; i++)
            {
                if (_grid[_gridHeight - 1][i] == 1)
                {
                    FloodFill(i, _gridHeight - 1, Solid, Empty);
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

        public double Evaluate(double aggregateHeightWeight, double completeLinesWeight, double holesWeight, double bumpinessWeight)
        {
            // Calculte aggregate height
            for (int i = 0; i < _gridWidth; i++)
            {
                int j = 0;
                while (j < _gridHeight && _grid[j][i] == Empty) { j++; }
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
                    if (_grid[y][x] != Empty && IsCompleteLine(y) == false)
                    {
                        block = true;
                    }
                    else if (_grid[y][x] == Empty && block)
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

            // Evaluate formula
            double evaluationValue =
                aggregateHeightWeight * agregateHeight +
                completeLinesWeight * completeLines +
                holesWeight * holes +
                bumpinessWeight * bumpines +
                0;
            return evaluationValue;
        }

        public void Draw(Bitmap bmp, float dotWith = 0.5f)
        {
            float xStep = bmp.Width / (float)_gridWidth;
            float yStep = bmp.Height / (float)_gridHeight;
            float halfXStep = xStep * dotWith;
            float halfYStep = yStep * dotWith;
            float offX = (xStep - halfXStep) / 2;
            float offY = (yStep - halfYStep) / 2;

            using (Pen borderPen = new Pen(Color.DarkGray))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    for (int x = 0; x < _gridWidth; x++)
                    {
                        Brush br = null;
                        if (_grid[y][x] == Empty)
                        {
                            br = Brushes.Black;
                        }
                        else if (_grid[y][x] == Solid)
                        {
                            br = Brushes.Blue;
                        }
                        else if (_grid[y][x] == ShapeA)
                        {
                            br = Brushes.Red;
                        }
                        else if (_grid[y][x] == ShapeB)
                        {
                            br = Brushes.Green;
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
