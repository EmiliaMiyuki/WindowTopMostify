﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowTopMost
{
    public partial class MiyukiListBox : ListBox
    {
        public ProcessHnd mouseItem;
        private MiyukiListBoxItemCollection m_Items;

        public MiyukiListBox() : base()
        {
            InitializeComponent();

            m_Items = new MiyukiListBoxItemCollection(this);

            base.DrawMode = DrawMode.OwnerDrawVariable;
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲   
            this.SetStyle(ControlStyles.ResizeRedraw, true); // 调整大小时重绘
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景. 
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true); // 开启控件透明
        }

        public new MiyukiListBoxItemCollection Items => m_Items;

        internal ListBox.ObjectCollection OldItemSource => base.Items;

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            for (int i = 0; i < Items.Count; i++)
            {
                Rectangle bounds = this.GetItemRectangle(i);
                Rectangle deleteBounds = new Rectangle(bounds.Width - 28, (bounds.Height - 16) / 2 + bounds.Top, 16, 16);

                if (bounds.Contains(e.X, e.Y))
                {
                    if (Items[i] != mouseItem)
                    {
                        mouseItem = Items[i];
                    }

                    if (deleteBounds.Contains(e.X, e.Y))
                    {
                        mouseItem.IsFocus = true;
                        //this.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        mouseItem.IsFocus = false;
                        //this.Cursor = Cursors.Arrow;
                    }

                    this.Invalidate();
                    break;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // you can set SeletedItem background
            if (this.Focused && this.SelectedItem != null)
            {
                
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Rectangle bounds = this.GetItemRectangle(i);

                if (mouseItem == Items[i])
                {
                    Color bgColor = Color.FromArgb(50, 255, 102, 153);

                    using (SolidBrush brush = new SolidBrush(bgColor))
                    {
                        g.FillRectangle(brush, new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height));
                    }
                }

                if (this.SelectedItem == Items[i])
                {
                    Color bgColor = Color.FromArgb(105, 255, 102, 153);

                    using (SolidBrush brush = new SolidBrush(bgColor))
                    {
                        g.FillRectangle(brush, new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height));
                    }
                }

                int fontLeft = bounds.Left + 15;
                System.Drawing.Font font = new System.Drawing.Font("微软雅黑", 11);
                System.Drawing.Font fontDesc = new System.Drawing.Font("微软雅黑", 9);

                g.DrawString(Items[i].WindowName, font, new SolidBrush(this.ForeColor), fontLeft, bounds.Top + 8);
                g.DrawString(Items[i].Handle.ToString(), fontDesc, new SolidBrush(Color.FromArgb(128, 128, 128)), fontLeft, bounds.Top + 31);

                //if (Items[i].Image != null)
                //{
                //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                //    g.DrawImage(Items[i].Image, new Rectangle(bounds.X + 5, (bounds.Height - 40) / 2 + bounds.Top, 40, 40));
                //}
                //g.DrawImage(Properties.Resources.error, new Rectangle(bounds.Width - 28, (bounds.Height - 16) / 2 + bounds.Top, 16, 16));
            }

            base.OnPaint(e);
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);
            if (Items.Count > 0)
            {
                ProcessHnd item = Items[e.Index];
                e.ItemHeight = 54;
            }

        }


        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.mouseItem = null;
            this.Invalidate();
        }

    }
}
