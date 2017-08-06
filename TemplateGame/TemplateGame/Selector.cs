//
//  Selector システム状態によって画面を遷移させるクラス
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TemplateGame
{
    //  ゲーム（システム）の状態
    enum GAMEPHASE{
        RESET = 0,
        INIT = 1,
        TITLE = 10,
        GAME  = 100,
        CLEAR = 200,
        GAMEOVER  = 210
    }

    class Selector
    {
        GAMEPHASE mPhase;
        GameScene mScene;
        MainWindow mApp;
        public Selector(MainWindow app){
            mPhase = GAMEPHASE.RESET;
            mApp = app;
            mScene = null;
        }
        public bool move(){
            SCENERESULT result;

            switch(mPhase){
                case GAMEPHASE.RESET:
                    mPhase = GAMEPHASE.INIT;
                    goto case GAMEPHASE.INIT;
                case GAMEPHASE.INIT:
                    mScene = new Title(this);
                    mPhase = GAMEPHASE.TITLE;
                    goto case GAMEPHASE.TITLE;
                case GAMEPHASE.TITLE:
                    if (mScene != null){
                        result = mScene.move();
                        if (result == SCENERESULT.DEFAULT){
                            break;
                        }
                        if (result == SCENERESULT.PROCEED){
                            mPhase = GAMEPHASE.GAME;
                            mScene = new Stage(this);
                            goto case GAMEPHASE.GAME;
                        }
                    }
                    break;
                case GAMEPHASE.GAME:
                    if (mScene != null){
                        result = mScene.move();
                        if (result == SCENERESULT.DEFAULT)
                            break;
                        mPhase = GAMEPHASE.CLEAR;
                        mScene = new Clear(this);
                        goto case GAMEPHASE.CLEAR;
                    }
                    break;
                case GAMEPHASE.CLEAR:
                    if (mScene != null){
                        result = mScene.move();
                        if (result == SCENERESULT.DEFAULT)
                            break;
                        mScene = new GameOver(this);
                        mPhase = GAMEPHASE.GAMEOVER;
                        goto case GAMEPHASE.GAMEOVER;
                    }
                    break;
                case GAMEPHASE.GAMEOVER:
                    if (mScene != null){
                        result = mScene.move();
                        if (result == SCENERESULT.DEFAULT)
                            break;
                        goto case GAMEPHASE.INIT;
                    }
                    break;
                    
                default:
                    break;
            }
            return true;
        }
        public void draw(DrawingContext dc){
            if (mScene != null){
                mScene.draw(dc);
            }
        }

        //  MainWindow を取得
        public MainWindow GetApp(){
            return mApp;
        }
    }
}
