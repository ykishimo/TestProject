//
//  Stage のひな形
//  リンクリストは独自実装ではなく、Collection のものを使用している
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
    class Stage : GameScene
    {
        BitmapImage mTex = null;
        List<GameObject> mObjects;
        public Stage(Selector sys) : base(sys){
            //  画像読み込み
            //  カレントディレクトリは、bin/debugフォルダなので1つ上にたどってから
            //  Media フォルダから画像ファイルを読む
            string cwd  = System.IO.Directory.GetCurrentDirectory();
            string path = System.IO.Directory.GetParent(cwd)+"\\..\\Media\\trump.gif";
            mTex = new BitmapImage(new Uri(path));    
            mObjects = new List<GameObject>();

            float angle = 0.0f;

            for (int i = 0; i < 4 ; ++i){
                mObjects.Add(new CardObject(this,mTex,angle));
                angle += 0.5f * (float)Math.PI;
            }
        }
        public override SCENERESULT move()
        {
            for (int i = mObjects.Count - 1 ; i >= 0 ; --i){
                if (!mObjects[i].move()){
                    mObjects.Remove(mObjects[i]);
                }
            }
            if (mObjects.Count == 0)
                return SCENERESULT.PROCEED;
            return SCENERESULT.DEFAULT;
        }
        public override void draw(DrawingContext dc)
        {
            foreach(GameObject obj in mObjects){
                obj.draw(dc);
            }
        }
    }
}
