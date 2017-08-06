//
//  ゲームクリアデモのひな形
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TemplateGame
{
    enum CLEARPHASE{
        INIT = 0,
        RUN  = 1,
        FADE = 2,
        DONE = 3
    }
    class Clear : GameScene
    {
        BitmapImage mTex = null;
        CLEARPHASE  mPhase;
        int         mFade;
        int			mLifetime;
        public Clear(Selector sys) : base(sys){
            //  画像読み込み
            //  カレントディレクトリは、bin/debugフォルダなので1つ上にたどってから
            //  Media フォルダから画像 ファイルを読む
            string cwd  = System.IO.Directory.GetCurrentDirectory();
            string path = System.IO.Directory.GetParent(cwd)+"\\..\\Media\\Clear.png";
            mTex = new BitmapImage(new Uri(path));        
            mPhase = CLEARPHASE.INIT;
            mFade = 0;
            mLifetime = 140;
        }
        public override SCENERESULT move(){
            switch (mPhase){
                case CLEARPHASE.INIT:
                    mPhase = CLEARPHASE.RUN;
                    goto case CLEARPHASE.RUN;
                case CLEARPHASE.RUN:
                    if (--mLifetime > 0)
                        break;
                    mPhase = CLEARPHASE.FADE;
                    break;
                case CLEARPHASE.FADE:
                    mFade++;
                    if (mFade < 100)
                        break;
                    goto case CLEARPHASE.DONE;
                case CLEARPHASE.DONE:
                    return SCENERESULT.PROCEED;
            }
            return SCENERESULT.DEFAULT;
        }
        public override void draw(DrawingContext dc)
        {
            MainWindow app = GetSelector().GetApp();
            float x = (float)(app.Width - mTex.Width) * 0.5f;
            float y = (float)(app.Height - mTex.Height) * 0.5f;
            dc.PushClip(new RectangleGeometry(new Rect(x,y,mTex.Width,mTex.Height)));
            dc.DrawImage(mTex,new Rect(x,y,mTex.Width,mTex.Height));
            dc.Pop();
            int alpha = Math.Min(mFade,100);
            Brush bg = new SolidColorBrush(Color.FromArgb((byte)((alpha * 255)/100),0,0,0));   //  背景色

            dc.DrawRectangle(bg,null,new Rect(0,0,app.Width,app.Height));
        }
    }
}
