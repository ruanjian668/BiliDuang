﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiliDuang.UI
{
    public partial class VideoList : UserControl
    {
        private List<VideoClass.episode> avList;
        private int moucecount = 0;

        public VideoList()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            panel2.Size = Size;
        }

        public void InitCards(List<VideoClass.episode> avs)
        {
            int lastx = 0, lasty = 0, i = 0;
            //DisableAllCards();
            avList = avs;
            foreach (VideoClass.episode av in avs)
            {
                AVCard card = new AVCard(av);
                card.Size = new Size(card.Size.Width + 10, card.Size.Height + 10);
                i++;

                if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    card.Location = new Point(lastx + 10, lasty);
                }
                else
                {
                    card.Location = new Point(lastx, lasty);
                }

                if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    lastx += card.Size.Width;
                }
                else
                {
                    lastx += 270;
                }

                panel2.Controls.Add(card);
                if (i == 4)
                {
                    if (Environment.OSVersion.Platform != PlatformID.Unix)
                    {
                        lasty = lasty + card.Size.Height + 70;
                    }
                    else
                    {
                        lasty = lasty + card.Size.Height;
                    }

                    lastx = 0;
                    i = 0;
                }
            }
            LoadCardsImages();
        }

        private void LoadCardsImages()
        {
            LoadCardsImages(0);
        }

        private async void LoadCardsImages(int start)
        {
            await Task.Run(() => {
                for (int offset = 0; start + offset < panel2.Controls.Count && offset < 8; offset++)
                {
                    if (start + offset < 0)
                    {
                        continue;
                    }

                    AVCard card = (AVCard)panel2.Controls[offset + start];
                    if (!card.imageloaded)
                    {
                        card.LoadImage();
                    }
                }
            });
                            
        }
        
        public void DisableAllCards()
        {
            panel2.Controls.Clear();
        }

        public void SetTipMessage(string message,bool visible = true)
        {
            materialLabel1.Text = message;
            materialLabel1.Visible = visible;
        }

        private void panel2_Scroll(object sender, ScrollEventArgs e)
        {
            moucecount = decimal.ToInt32(Math.Floor((decimal)(e.NewValue / 400)));
            LoadCardsImages(moucecount * 4);
        }

        private void mousewheel(object sender, MouseEventArgs e)
        {

            if (e.Delta < 0)
            {
                moucecount++;
            }
            else
            {
                if (moucecount <= 0)
                {
                    return;
                }

                moucecount--;
            }
            LoadCardsImages(moucecount * 4);
        }
    }
}
