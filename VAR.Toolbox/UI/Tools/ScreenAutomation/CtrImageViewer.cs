using System;
using System.Drawing;
using System.Windows.Forms;

namespace VAR.ScreenAutomation.Controls
{
    public class CtrImageViewer : PictureBox
    {
        #region Declarations

        private Image _imageShow;

        #endregion

        #region Properties

        public Image ImageShow
        {
            // ReSharper disable once InconsistentlySynchronizedField
            get => _imageShow;
            set
            {
                lock (this)
                {
                    _imageShow = value;
                    Invalidate();
                }
            }
        }

        #endregion

        #region Control life cycle

        public CtrImageViewer()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.Black;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Redraw(pe.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //Redraw(null);
            this.Invalidate();
        }

        #endregion

        #region Private methods

        private void Redraw(Graphics graph)
        {
            if (_imageShow == null)
            {
                return;
            }

            lock (_imageShow)
            {
                if (graph == null)
                {
                    graph = this.CreateGraphics();
                }

                // Calcular dimensiones a dibujar y centrar
                int imgDrawWidth;
                int imgDrawHeight;
                float imgDrawX = 0;
                float imgDrawY = 0;
                float relation = _imageShow.Width / (float)_imageShow.Height;
                if (relation > 0)
                {
                    // Imagen mas ancha que alta
                    imgDrawHeight = (int)(Width / relation);
                    if (imgDrawHeight > Height)
                    {
                        imgDrawHeight = Height;
                        imgDrawWidth = (int)(Height * relation);
                        imgDrawX = ((Width - imgDrawWidth) / 2.0f);
                    }
                    else
                    {
                        imgDrawWidth = Width;
                        imgDrawY = ((Height - imgDrawHeight) / 2.0f);
                    }
                }
                else
                {
                    // Imagen mas alta que ancha
                    imgDrawWidth = (int)(Width * relation);
                    if (imgDrawWidth > Width)
                    {
                        imgDrawWidth = Width;
                        imgDrawHeight = (int)(Height / relation);
                        imgDrawY = ((Height - imgDrawHeight) / 2.0f);
                    }
                    else
                    {
                        imgDrawHeight = Height;
                        imgDrawX = ((Width - imgDrawWidth) / 2.0f);
                    }
                }

                graph.DrawImage(_imageShow, imgDrawX, imgDrawY, imgDrawWidth, imgDrawHeight);
            }
        }

        #endregion
    }
}