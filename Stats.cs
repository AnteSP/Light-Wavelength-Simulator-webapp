using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public static Stats s;

    public bool trueColor = true;

    [SerializeField] Image I;
    Image R, G, B,R2,G2,B2;
    Text RT, GT, BT;
    Renderer[] Ren;
    [SerializeField] GameObject[] SubjectsEXT;
    [SerializeField] Image[] LightSourcesEXT;
    int crsr = 0,crsrS = 0;
    Hashtable Subjects = new Hashtable();
    Hashtable LightSources = new Hashtable();
    Animation anim;
    Reflector temp;
    [SerializeField] Slider slid;
    [SerializeField] Text subj;

    float tempVar = 1;
    
    public string lightSource, reflector;

    float ctrOff = (0.55f - 3.5f) / -3.2f;
    float redOff = ((0.65f - 3.5f) / -3.2f) - ((0.55f - 3.5f) / -3.2f);
    float grnOff = ((0.525f - 3.5f) / -3.2f) - ((0.55f - 3.5f) / -3.2f);
    float bluOff = ((0.475f - 3.5f) / -3.2f) - ((0.55f - 3.5f) / -3.2f);

    [SerializeField] GameObject[] txtBoxes;

    //
    //0.3 - 3.5    -3.2 - 0      1 - 0
    //RED = 0.65 GREEN = 0.525 BLUE = 0.475 
    //CENTRE = 0.55
    //RED = 53/48 GREEN = 85/96 BLUE = 383/480 (anim time)
    //CENTRE = 913/960
    //RED = +49/320 GREEN = -21/320 BLUE = -49/320

    // 0.08768626    14.3989     5.382431

    // Start is called before the first frame update
    void Start()
    {
        //1 - 0
        //0.3 1.9 3.5
        //0.3 1.1 1.9 2.7 3.5
        float dataBegin = 0.3f;
        dataBegin = ((dataBegin - 3.5f) / -3.2f) * (3f / 5f);
        float dataEnd = 2f;
        dataEnd = ((dataEnd - 3.5f) / -3.2f) * (3f / 5f);

        print("START: " + dataBegin + " INB: " + ((3*dataBegin) + dataEnd) / 4f + " MID: " + (dataBegin+dataEnd)/2f +" INB: " + (dataBegin + (dataEnd*3)) / 4f + " END: " + dataEnd);

        R = I.transform.parent.Find("R").GetComponent<Image>();
        G = I.transform.parent.Find("G").GetComponent<Image>();
        B = I.transform.parent.Find("B").GetComponent<Image>();
        R2 = I.transform.parent.Find("R (1)").GetComponent<Image>();
        G2 = I.transform.parent.Find("G (1)").GetComponent<Image>();
        B2 = I.transform.parent.Find("B (1)").GetComponent<Image>();
        RT = I.transform.parent.Find("RT").GetComponent<Text>();
        GT = I.transform.parent.Find("GT").GetComponent<Text>();
        BT = I.transform.parent.Find("BT").GetComponent<Text>();

        foreach (GameObject g in SubjectsEXT)
        {
            Subjects.Add(g.name, g);
        }
        foreach(Image i in LightSourcesEXT)
        {
            LightSources.Add(i.gameObject.name, i);
        }
    
        s = this;
        anim = GetComponent<Animation>();
        temp = GetComponent<Reflector>();

        setSource(lightSource = LightSourcesEXT[0].name);
        setSubject(reflector = SubjectsEXT[0].name);
        crsr = 0;
        setColors(slid.value);
    }

    float getIntensity(string lightS,string refl,float v)
    {
        anim.GetClip(refl).SampleAnimation(gameObject, v);
        float outp = temp.Reflectance;
        anim.GetClip(lightS).SampleAnimation(gameObject, v);
        print("SUBJ: " + outp + "   SUN: " + temp.Reflectance);
        return (outp * temp.Reflectance);
    }

    void setSource(string s)
    {
        ((Image)LightSources[lightSource]).gameObject.SetActive(false);
        lightSource = s;
        ((Image)LightSources[lightSource]).gameObject.SetActive(true);
    }

    void setSubject(string s)
    {
        ((GameObject)Subjects[reflector]).SetActive(false);
        reflector = s;
        ((GameObject)Subjects[reflector]).SetActive(true);
        Ren = ((GameObject)Subjects[reflector]).GetComponentsInChildren<Renderer>();
        subj.text = "Displaying: " + ((GameObject)Subjects[reflector]).name;
    }

    //0.3 - 3.5
    //RED = 0.65 GREEN = 0.525 BLUE = 0.475 
    //CENTRE = 0.55
    //RED = 53/48 GREEN = 85/96 BLUE = 383/480 (anim time)
    //CENTRE = 913/960
    //RED = +49/320 GREEN = -21/320 BLUE = -49/320

    // 0.08768626    14.3989     5.382431

    //1 -> 0.4 -> 2/3 = 0.666666666
    //0.9 -> 0.6 -> 1
    //0 -> 2.4 -> 4 + 1/6 = 25/6 = 4.16666666

    public void setColors(float b)
    {
        I.color = new Color( getIntensity(lightSource,reflector,b+redOff) , getIntensity(lightSource,reflector,b+grnOff) , getIntensity(lightSource, reflector, b+bluOff) );
        R.color = new Color(I.color.r, 0, 0);
        G.color = new Color(0, I.color.g, 0);
        B.color = new Color(0, 0, I.color.b);
        tempVar = I.color.maxColorComponent;
        I.color = I.color / I.color.maxColorComponent;
        R2.color = new Color(I.color.r, 0, 0);
        G2.color = new Color(0, I.color.g, 0);
        B2.color = new Color(0, 0, I.color.b);
        I.color = I.color * (trueColor ? tempVar : 1);
        RT.text = Mathf.RoundToInt(I.color.r*100) + "%";
        GT.text = Mathf.RoundToInt(I.color.g * 100) + "%";
        BT.text = Mathf.RoundToInt(I.color.b * 100) + "%";
        foreach (Renderer r in Ren)
        {
            r.material.color = I.color;
        }
    }

    public void darkMode()
    {
        Camera.main.backgroundColor = Camera.main.backgroundColor.Equals(Color.black) ? Color.white : Color.black;
    }

    public void trueColorF()
    {
        trueColor = !trueColor;
        setColors(slid.value);
    }

    public void cycleSub()
    {
        setSubject(SubjectsEXT[++crsr % SubjectsEXT.Length].name);
        setColors(slid.value);
    }

    public void cycleSrce()
    {
        setSource(LightSourcesEXT[++crsrS % LightSourcesEXT.Length].name);
        setColors(slid.value);
    }

    public void cycleText()
    {

        for(int i = 0; i < txtBoxes.Length-2; i++)
        {
            if (txtBoxes[i].activeSelf)
            {
                txtBoxes[i].SetActive(false);
                txtBoxes[i+1].SetActive(true);
                return;
            }
        }

        txtBoxes[txtBoxes.Length - 1].SetActive(false);
    }
}
