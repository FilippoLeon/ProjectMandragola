using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public class SpriteLoader : MonoBehaviour {

    static string folder = "Sprites";

    static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    //public List< Sprite> sprites_____;
    public Vector2 defaultSize = new Vector2(64,64);
    public Vector2 defaultPivot = new Vector2(0.5f, 0.5f);

    public Texture2D placeholderTexture;
    static public Sprite placeHolder;

    public class SpriteData
    {
        public string name;
        public Rect rect;
        public Vector2 pivot;
        //private Vector2 vector2;

        public SpriteData() { }
        public SpriteData(Rect rect, Vector2 pivot)
        {
            this.rect = rect;
            this.pivot = pivot;
        }
        
    }

	// Use this for initialization
	void Start () {
       DirectoryInfo dir = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, folder));
       FileInfo[] fileInfo = dir.GetFiles("*.png");
        foreach (FileInfo file in fileInfo) {
            loadSpriteSheet(file);
        }

        if (placeHolder != null) return;
        int sizeX = 32, sizeY = 32;
        placeholderTexture = new Texture2D(sizeX, sizeY, TextureFormat.RGBA32, false);
        placeholderTexture.filterMode = FilterMode.Point;

        Color[] colors = new Color[] { new Color(247f / 255f, 152f / 255f, 19f/255f),
                        new Color(247f/255f, 19f/255f, 98f/255f) };
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                int color = 0;
                if ((i / 16 + j / 16) % 2 == 0 ) color = 1;
                placeholderTexture.SetPixel(i, j, colors[color]);
            }

        }
        placeholderTexture.Apply();

        placeHolder = Sprite.Create(placeholderTexture, 
            new Rect(0f, 0f, sizeX, sizeY), 
            new Vector2(0.5f, 0.5f));

        //textures_____ = new List<Texture>();
        //sprites_____ = sprites.Select(d => d.Value).ToList();
    }
    //public List<Texture> textures_____ = new List<Texture>();

    void loadSpriteSheet(FileInfo path)
    {
        string[] split = path.ToString().Split('.');
        string pathXml = string.Join(".", split, 0, split.Length - 1) + ".xml";
        string fileName = path.Name.Split('.')[0];
        //File.Name

        Debug.Log(string.Format("Loading \"{0}\" ({1})...", path, fileName));


        byte[] imageData = File.ReadAllBytes(path.ToString());
        
        Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        //textures_____.Add(tex);
        
        if (tex.LoadImage(imageData))
        {
            tex.filterMode = FilterMode.Point;

            //Texture2D tex = Resources.Load(path.ToString()) as Texture2D;
        }  else
        {
            Debug.LogError("Cannot load texture!");
        }
        //Debug.Assert(Resources.Load(path.ToString()) != null, "Resource can't be null, invalid file name?");
        Debug.Assert(tex != null, "Texture can't be null, invalit sheet map?");

        //Debug.Log(string.Format("Opening sprite .xml file \"{0}\" ({1})...", pathXml, fileName));
        if ( File.Exists(pathXml) ) {
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader = XmlReader.Create(pathXml, settings);

            loadSprites(reader, tex);
        } else
        {
            SpriteData data = new SpriteData(new Rect(Vector2.zero, defaultSize), defaultPivot);
            data.name = name;
            sprites[data.name] = Sprite.Create(tex, data.rect, data.pivot);
            return;
        }
    }

    public void loadSprites(XmlReader reader, Texture2D tex)
    {
        reader.MoveToContent();
        //reader.ReadToNextSibling("SpriteSheet");
        Vector2 dP = defaultPivot;
        Vector2 dS = defaultSize;
        if (reader.GetAttribute("defaultSize") != null)
        {
            string[] size = reader.GetAttribute("defaultSize").Split(',');
            dS = new Vector2(Convert.ToSingle(size[0]), Convert.ToSingle(size[1]));
        }
        if (reader.GetAttribute("defaultPivot") != null)
        {
            string[] pivot = reader.GetAttribute("defaultPivot").Split(',');
            dP = new Vector2(Convert.ToSingle(pivot[0]), Convert.ToSingle(pivot[1]));
        }

        while (reader.Read())
        {
            XmlNodeType nodeType = reader.NodeType;
            switch (nodeType) {
                case XmlNodeType.Element:
                    Debug.Assert(reader.Name == "Sprite");
                    Vector2 pos;
                    if (reader.GetAttribute("start") != null)
                    {
                        string[] start = reader.GetAttribute("start").Split(',');
                        pos = new Vector2(Convert.ToSingle(start[0]), Convert.ToSingle(start[1]));
                    } else
                    {
                        pos = new Vector2(0,0);
                    }
                    Vector2 dS1 = dS;
                    if (reader.GetAttribute("size") != null)
                    {
                        string[] size = reader.GetAttribute("size").Split(',');
                        dS1 = new Vector2(Convert.ToSingle(size[0]), Convert.ToSingle(size[1]));
                    }
                    Vector2 dP1 = dP;
                    if (reader.GetAttribute("pivot") != null)
                    {
                        string[] pivot = reader.GetAttribute("pivot").Split(',');
                        dP1 = new Vector2(Convert.ToSingle(pivot[0]), Convert.ToSingle(pivot[1]));
                    }
                    string name = reader.ReadInnerXml();
                    //pos.x /= tex.width;
                    //pos.y /= tex.height;
                    //dS1.x /= tex.width;
                    //dS1.y /= tex.height;
                    //Debug.Log(string.Format("Loading sprite {0} x: {1} size: {2} piv: {3}",
                                    //name, pos, dS1, dP1));
                    Debug.Assert(tex != null);
                    sprites[name] = Sprite.Create(tex, new Rect(pos, dS1), dP1, 1);
                    Debug.Assert(sprites[name] != null);
                    break;
                default:
                    break;
            }
        }

    }

    public static Sprite getSprite(string name)
    {
        if (sprites.ContainsKey(name) && sprites[name] != null) return sprites[name];
        else return placeHolder;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
