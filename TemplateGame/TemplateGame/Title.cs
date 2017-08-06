//
//  タイトル画面のひな形
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
    enum TITLEPHASE{
        INIT = 0,
        RUN  = 1,
        FADE = 2,
        DONE = 3
    }
    class Title : GameScene
    {
        BitmapImage mTex = null;
        int         mCount;
        TITLEPHASE  mPhase;
        int         mFreq;
        int         mActive;
        int         mFade;
        bool        mKeyFlag;
        public Title(Selector sys) : base(sys){
            //  画像読み込み
            //  カレントディレクトリは、bin/debugフォルダなので2つ上にたどってから
            //  Media フォルダから画像 ファイルを読む
            string cwd  = System.IO.Directory.GetCurrentDirectory();
            string path = System.IO.Directory.GetParent(cwd)+"\\..\\Media\\title.png";
            mTex = new BitmapImage(new Uri(path));        
            mPhase = TITLEPHASE.INIT;
            mFreq = 120;
            mActive = 60;
            mFade = 0;
            mKeyFlag = true;
        }
        //  アニメーション
        //  スペースキーが押されると、次へ進む
        public override SCENERESULT move(){
            switch (mPhase){
                case TITLEPHASE.INIT:
                    mPhase = TITLEPHASE.RUN;
                    mKeyFlag = true;
                    goto case TITLEPHASE.RUN;
                case TITLEPHASE.RUN:
                    if (KeyState.Space){
                        if (!mKeyFlag){
                            mFade = 0;
                            mPhase = TITLEPHASE.FADE;
                            mFreq = 15;
                            mActive = 10;
                            break;
                        }
                        mKeyFlag = true;
                    }else{
                        mKeyFlag = false;
                    }
                    break;
                case TITLEPHASE.FADE:
                    mFade++;
                    if (mFade < 100)
                        break;
                    goto case TITLEPHASE.DONE;
                case TITLEPHASE.DONE:
                    return SCENERESULT.PROCEED;
            }
            mCount = (mCount + 1) % mFreq;
            return SCENERESULT.DEFAULT;
        }
        public override void draw(DrawingContext dc)
        {
            MainWindow app = GetSelector().GetApp();
            float x = (float)(app.Width - mTex.Width) * 0.5f;
            float y = (float)(app.Height - mTex.Height*0.5f) * 0.5f;
            dc.PushClip(new RectangleGeometry(new Rect(x,y,mTex.Width,mTex.Height*0.5)));
            dc.DrawImage(mTex,new Rect(x,y,mTex.Width,mTex.Height));
            dc.Pop();
            if (mCount < mActive){
                y += 96.0f;
                dc.PushClip(new RectangleGeometry(new Rect(x,y,mTex.Width,mTex.Height*0.25)));
                dc.DrawImage(mTex,new Rect(x,y-mTex.Height*0.5f,mTex.Width,mTex.Height));
                dc.Pop();
            }
            int alpha = Math.Min(mFade,100);
            Brush bg = new SolidColorBrush(Color.FromArgb((byte)((alpha * 255)/100),0,0,0));   //  背景色

            dc.DrawRectangle(bg,null,new Rect(0,0,app.Width,app.Height));
        }
    }
}
