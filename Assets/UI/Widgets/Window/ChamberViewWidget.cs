using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChamberViewWidget : MonoBehaviour {
    public class Seat
    {
        public int seatNo; // Sequential position
        public int pos; // Pos within row
        public Row parent;

        public Seat(int i, int globi, Row parent_)
        {
            seatNo = globi;
            pos = i;
            parent = parent_;
            //Debug.Log(string.Format("NEW seat {0} local {1} in row {2} local {3} is section {4} at cart {5}, polar {6}", globi, i,
            //    parent.pos, parent.rowNo,
            //    parent.parent.sectionNo,
            //    this.parent.parent.parent.getPos(this.parent.parent.parent.getPolarPos(this)), 
            //    this.parent.parent.parent.getPolarPos(this)));
        }
    }

    public class Row
    {
        public int rowNo; // Sequential position
        public int pos; // Pos within section
        public Section parent;
        public Seat[] seats;

        // Geometry
        public float arcLength; // Arc length of row

        public Row(int i, int globi, int nseats, Section parent_)
        {
            rowNo = globi;
            pos = i;
            parent = parent_;
            seats = new Seat[nseats];
            //Debug.Log(string.Format("NEW row {0} local {1} (seats: {2}) in section {3} ",
            //    globi, 
            //    i,
            //    seats.Length,
            //    parent.sectionNo));
        }
    }

    public class Section
    {
        public int sectionNo; // Sequential Section number
        public Row[] rows;
        // Redundant
        public int noSeats = 0;
        public ChamberViewWidget parent;

        // Geometry
        public float arc;
        public float arcStart;

        public Section(int i, int nrows, ChamberViewWidget parent_) {
            sectionNo = i;
            rows = new Row[nrows];
            parent = parent_;
            //Debug.Log(string.Format("NEW sec {0} (rows: {1}) start",
                //i,
                //nrows));
        }
    }

    Section[] sections;
    float arc = 0f;

    public int seats = 40;

    // Geometry
    [Range(0, 20)]
    public int layers = 4;
    [Range(0, Mathf.PI*2)]
    public float totArc = Mathf.PI; // Arc including spaces
    [Range(0, 20)]
    public float rMin = 4, rSize = 4;
    public float[] sectionArcs = { 1, 1, 1 }; // arc length for each section
                                              //public 

    List<GameObject> buffer = new List<GameObject>();
    int usedBuffer = 0;

    public GameObject seatPrototype;
    float startAngle = 0;
    float centerOffset = 0;

    public GameObject centeredObject;

	// Use this for initialization
	void Start () {
        populate();
	}

    public GameObject GetFromBuffer()
    {
        while(usedBuffer >= buffer.Count)
        {
            buffer.Add(Instantiate(seatPrototype));
        }
        return buffer[usedBuffer++];
    }

    public void repopulate()
    {
        usedBuffer = 0;
        for (int i = 0; i < buffer.Count; i++)
        {
            buffer[i].SetActive(false);
        }
        populate();
    }

    public void populate()
    {
        arc = 0f;
        foreach (float sec in sectionArcs)
        {
            Debug.Assert(sec > 0);
            arc += sec;
        }
        if(arc > totArc) totArc = arc;

        startAngle = (totArc + Mathf.PI) / 2f;
        if (totArc <= Mathf.PI) centerOffset = 0.5f * (rMin + rSize);
        else centerOffset = 0.5f * (rMin + rSize) - Mathf.Sin((totArc - Mathf.PI) * 0.5f) * 0.5f * (rMin + rSize);


        int nsections = sectionArcs.Length;
        int nrows = 0;
        int nseats = 0;

        sections = new Section[nsections];
        System.Random nrd = new System.Random();

        int[] allocSeats = new int[nsections];
        int totAllocSeats = 0;
        for (int i = 0; i < nsections; i++)
        {
            sections[i] = new Section(i, layers, this);

            sections[i].arc = sectionArcs[i];
            if (i > 0)
                sections[i].arcStart = (totArc - arc) / (nsections - 1)
                    + sections[i - 1].arcStart + sections[i - 1].arc;
            else
                sections[i].arcStart = 0;
            allocSeats[i] = Mathf.FloorToInt((float)seats * sectionArcs[i] / arc); // Some seat lost
            totAllocSeats += allocSeats[i];
        }
        int remainderAllocSeats = seats - totAllocSeats;
        //Debug.Log(string.Format("Section {0} data: start {1} len {2}",
        //i, sections[i].arcStart, sections[i].arc));

        for (int i = 0; i < nsections; i++)
        {
            sections[i].noSeats = allocSeats[i] + (remainderAllocSeats-- > 0 ? 1 : 0);

            float[] rowArcLengths = new float[layers];
            float r = rMin, totLength = 0;
            for (int j = 0; j < layers; j++)
            {
                rowArcLengths[j] = arclength(r, sections[i].arc);
                totLength += rowArcLengths[j];
                r += (float)rSize / (layers - 1);
            }

            int scale = 20;

            int totAllocSeatForSection = 0;
            Color col = new Color((float)nrd.NextDouble(), (float)nrd.NextDouble(), (float)nrd.NextDouble());
            int[] allocSeat = new int[layers];
            for (int j = 0; j < layers; j++)
            {
                float rowPercArcLength = rowArcLengths[j] / totLength;
                allocSeat[j] = Mathf.FloorToInt(rowPercArcLength * sections[i].noSeats);
                totAllocSeatForSection += allocSeat[j];
            }
            int remainingSeatsForSection = sections[i].noSeats - totAllocSeatForSection;
            //Debug.Log(remainingSeatsForSection);
            for (int j = 0; j < layers; j++)
            {
                allocSeat[j] += remainingSeatsForSection-- > 0 ? 1 : 0;
                sections[i].rows[j] = new Row(j, nrows++, allocSeat[j],
                sections[i]); // Some seat lost


                for (int k = 0; k < sections[i].rows[j].seats.Length; k++)
                {
                    Seat s = new Seat(k, nseats++, sections[i].rows[j]);
                    sections[i].rows[j].seats[k] = s;

                    Vector2 polar = getPolarPos(sections[i].rows[j].seats[k]);
                    Vector2 xyPos = getPos(polar);
                    GameObject newSeat = GetFromBuffer();
                    newSeat.transform.SetParent(this.transform);
                    newSeat.SetActive(true);
                    newSeat.name = s.pos + "_" + s.parent.rowNo + "_" + s.parent.parent.sectionNo + "-" + s.seatNo;
                    Vector2 nP = xyPos * scale;
                    nP.y -= centerOffset * scale;
                    newSeat.transform.localPosition = nP;
                    //newSeat.transform.rotation = Quaternion.Euler(0f, 0f, - (Mathf.PI / 2f - polar.y) / Mathf.PI * 180);
                    newSeat.GetComponent<Image>().color = col;
                }
            }
            Debug.Assert(remainingSeatsForSection <= 0);

            if(centeredObject != null)
            {
                Vector2 pos = centeredObject.transform.localPosition;
                pos.y = - centerOffset * scale;
                centeredObject.transform.localPosition = pos;
            }

            int pad = 0;
            (transform as RectTransform).sizeDelta = new Vector2(2 * ((rSize + rMin) * scale + pad),
                                            ((rMin + rSize) - 2 * centerOffset) * scale + (rMin + rSize) * scale + 2 * pad);
        }
        //int rowLenght = seats / layers;

    }

    public Vector2 getPolarPos(Seat s)
    {
        float r = rMin + s.parent.pos * rSize / (layers - 1);
        float angle = startAngle - s.parent.parent.arcStart - s.pos * s.parent.parent.arc / (s.parent.seats.Length - 1);
        //Debug.Log(string.Format("Polar : arcs {0} and arc {1} and seat {2} len {3} and {4}", 
        //    s.parent.parent.arcStart, s.parent.parent.arc, s.pos, s.parent.seats.Length, angle));
        return new Vector2(r, angle);
    }

    public Vector2 getPos(Vector2 inv)
    {
        float r = inv.x;
        float angle = inv.y;
        return new Vector2(r * Mathf.Cos(angle), r * Mathf.Sin(angle));
    }

    float arclength(float r, float sec)
    {
        return r * sec;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
