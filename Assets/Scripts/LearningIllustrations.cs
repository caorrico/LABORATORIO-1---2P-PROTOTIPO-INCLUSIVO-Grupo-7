using UnityEngine;
namespace AulaInclusiva
{
    // Original schematic illustrations. These are learning aids, not geographic maps or signs.
    public static class LearningIllustrations
    {
        public static Texture2D Create(int level, int card) {
            var texture = new Texture2D(320,144,TextureFormat.RGB24,false);
            Color sky = new Color(.68f,.86f,.93f), sea = new Color(.06f,.38f,.65f), green = new Color(.12f,.48f,.27f), sand = new Color(.86f,.73f,.40f);
            var pixels = new Color[320*144]; for (int i=0;i<pixels.Length;i++) pixels[i] = sky;
            void Rect(int x,int y,int w,int h,Color color) {
                for (int py=Mathf.Max(0,y);py<Mathf.Min(144,y+h);py++)
                    for(int px=Mathf.Max(0,x);px<Mathf.Min(320,x+w);px++) pixels[py*320+px]=color;
            }
            void Triangle(int x,int y,int w,int h,Color color) {
                for (int row=0;row<h;row++) { int half=Mathf.RoundToInt(w*.5f*(1f-(float)row/h)); Rect(x+w/2-half,y+row,half*2+1,1,color); }
            }
            if (level==0 && card==0) {
                Rect(0,0,320,65,sea); Rect(0,65,320,20,sand);
                for(int x=0;x<300;x+=60) { Rect(x,22,35,3,Color.white); Rect(x+20,45,35,3,Color.white); }
            } else if(level==0 && card==1) {
                Rect(0,0,320,25,green); Triangle(20,20,150,110,new Color(.35f,.40f,.46f)); Triangle(135,20,170,95,new Color(.28f,.34f,.39f));
                Triangle(70,94,49,36,Color.white);
            } else if(level==0 && card==2) {
                Rect(0,0,320,35,green); Rect(130,0,50,42,sea);
                for(int x=20;x<320;x+=65) { Rect(x+15,25,12,65,new Color(.36f,.22f,.12f)); Triangle(x,53,48,65,green); }
            } else if((level==0 && card==3) || (level==1 && card==2)) {
                Rect(0,0,320,100,sea); Rect(35,60,80,17,sand); Triangle(45,77,60,35,green);
                Rect(180,25,105,20,sand); Triangle(195,45,65,30,green);
            } else if(level==1) {
                Rect(0,0,320,22,new Color(.45f,.48f,.5f));
                for(int x=25;x<300;x+=65) {
                    Rect(x,22,48,55,sand); Triangle(x-3,77,54,28,new Color(.62f,.24f,.16f));
                    Rect(x+17,22,14,30,new Color(.22f,.27f,.32f)); Rect(x+7,57,10,12,sea); Rect(x+30,57,10,12,sea);
                }
            } else if(card==0) {
                Rect(40,30,240,20,sand); Rect(100,50,120,63,sand); Rect(100,50,120,16,new Color(.30f,.22f,.13f));
                for(int x=110;x<220;x+=12) Rect(x,75,2,32,new Color(.56f,.42f,.22f));
            } else if(card==1) {
                Rect(85,15,150,115,Color.white); for(int y=35;y<115;y+=18) Rect(105,y,105,4,sea);
            } else {
                Rect(20,20,85,95,sand); Rect(118,20,85,95,green); Rect(216,20,85,95,sea);
                for(int x=43;x<300;x+=98) { Rect(x,45,40,7,Color.white); Rect(x,65,40,7,Color.white); Rect(x,85,40,7,Color.white); }
            }
            texture.SetPixels(pixels); texture.Apply(); texture.filterMode=FilterMode.Point; return texture;
        }
    }
}
