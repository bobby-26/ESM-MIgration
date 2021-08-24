using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;

public partial class UserControlRHTimeList : System.Web.UI.UserControl
{
    public event EventHandler TimeStripCommand;
    string _fieldtobind = "FLDHOURSATSEA";
    string _fieldValue = "FLDRESTHOURWORKID";
    decimal _workhours;
    string _id;
    int _itemindex;
    decimal _totalhours  = 24;
    decimal _tworkhour = 0;
    decimal _tresthour = 0;
    bool _showTotalHour = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //bind();
        }
    }

    public void SetTimeList(DataTable dt)
    {
        if (!ShowTotalHours)
        {
            dlstTimeList.DataSource = dt;
            dlstTimeList.DataBind();
            return;
        }
        DataRow r = dt.NewRow();
        r[FieldToBind] = 12.2;
        r[FieldValue] = Guid.NewGuid();
        dt.Rows.Add(r);
        dlstTimeList.DataSource = dt;
        dlstTimeList.DataBind();
        dt.Rows.Remove(r);
        Button b = (Button)dlstTimeList.Items[24].FindControl("btnTimeEntry");
        if (b != null)
            b.Visible = false;
        Label lbl25text = (Label)dlstTimeList.Items[24].FindControl("lblsno");
        if (lbl25text != null)
        {
            lbl25text.Text = "";       
            lbl25text.Text = "<font color=\"red\"><b>T.H :</b></font>" + "<font color=\"MediumOrchid \">&nbsp;<b>" + TotalHours + "</b> </font>" + "<br/><hr / style=\"margin: 0\"><font color=\"red\"><b>R.H :</b></font>" + "<font color=\"MediumOrchid \">&nbsp;<b>" + TRestHours + "</b> </font>" + " </font>" + "<br/><hr / style=\"margin: 0\"><font color=\"red\"><b>W.H :</b></font>" + "<font color=\"MediumOrchid \">&nbsp;<b>" + TWorkHours + "</b> </font>";
        }
    }
    public decimal WorkHours
    {
        get
        {
            return _workhours;
        }
        set
        {
            _workhours = value;
        }
    }
    public string  Id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }
    public int ItemIndex
    {
        get
        {
            return _itemindex;
        }
        set
        {
            _itemindex = value;
        }
    }
    public decimal TotalHours
    {
        get
        {
            return _totalhours;
        }
        set
        {
            _totalhours = value;
        }
    }
    public decimal TWorkHours
    {
        get
        {
            return _tworkhour;
        }
        set
        {
            _tworkhour = value;
        }
    }
    public decimal TRestHours
    {
        get
        {
            return _tresthour;
        }
        set
        {
            _tresthour = value;
        }
    }
    public string FieldToBind
    {
        get
        {
            return _fieldtobind;
        }
        set
        {
            _fieldtobind = value;
        }
    }
    public string FieldValue
    {
        get
        {
            return _fieldValue;
        }
        set
        {
            _fieldValue = value;
        }
    }
    public bool ShowTotalHours
    {
        get
        {
            return _showTotalHour;
        }
        set
        {
            _showTotalHour = value;
        }
    }
    protected void dlstTimeList_ItemCommand(object sender, DataListCommandEventArgs dce)
    {
        Button btn = (Button)dce.Item.FindControl("btnTimeEntry");
        TextBox txtHours = (TextBox)dce.Item.FindControl("txtWorkHours");
        TextBox txtid = (TextBox)dce.Item.FindControl("txtid");

        _itemindex = dce.Item.ItemIndex + 1;
        SetWorkHourEntry(btn, txtHours,txtid);
        BindValues(btn, dce.Item.ItemIndex);
        OnTimeStripCommand(dce);

    }
    private void SetCurrentWorkHourEntry(Button btn, TextBox txtHours)
    {
        decimal n = 3;
            decimal.TryParse (txtHours.Text,out n);

        if (n.Equals(0))
        {
            btn.BackColor = System.Drawing.Color.White;
            btn.Text = "0.0";
        }

        if (n.Equals((decimal)1))
        {
            btn.BackColor = System.Drawing.Color.LightGray;
            btn.Text = "1.0";
        }

        if (n.Equals((decimal)0.5))
        {
            btn.BackColor = System.Drawing.Color.Yellow;
            btn.Text = "0.5";
        }       
    }
    private void BindValues(Button btn,int indexpo)
    {
        string[] advancewithhalf = { "0.83", "0.83", "0.84", "23.5" };
        string[] advancewithone = { "0.67", "0.67", "0.66", "23" };
        string[] advancewithtwo = { "0.33", "0.33", "0.34", "22" };

        string[] retardwithhalf = { "1.17", "1.17", "1.16", "24.5" };
        string[] retardwithone = { "1.33", "1.33", "1.34", "25" };
        string[] retardwithtwo = { "1.67", "1.67", "1.66", "26" };

        if (TotalHours.ToString() == advancewithone[3].ToString() || TotalHours.ToString() == "23.00")
            BindDefault(advancewithone, btn, indexpo);

        if (TotalHours.ToString() == advancewithhalf[3].ToString() || TotalHours.ToString() == "23.50")
            BindDefault(advancewithhalf,btn, indexpo);

        if (TotalHours.ToString() == retardwithone[3].ToString() || TotalHours.ToString() == "25.00")
            BindDefault(retardwithone, btn, indexpo);

        if (TotalHours.ToString() == retardwithhalf[3].ToString() || TotalHours.ToString() == "24.50")
            BindDefault(retardwithhalf,btn,indexpo);

        if (TotalHours.ToString() == retardwithtwo[3].ToString() || TotalHours.ToString() == "26.00")
            BindDefault(retardwithtwo, btn, indexpo);

        if (TotalHours.ToString() == advancewithtwo[3].ToString() || TotalHours.ToString() == "22.00")
            BindDefault(advancewithtwo, btn, indexpo);
    }
    private void BindDefault(string[] val,Button btn,int indexpo)
    {
        if (btn != null && indexpo.ToString() == "3")
        {
            btn.Text = val[0].ToString();
            btn.ForeColor = System.Drawing.Color.Red;
        }        

        if (btn != null && indexpo.ToString() == "7")
        {
            btn.Text = val[1].ToString();
            btn.ForeColor = System.Drawing.Color.Red;
        }       

        if (btn != null && indexpo.ToString() == "21")
        {
            btn.Text = val[2].ToString();
            btn.ForeColor = System.Drawing.Color.Red;
        }       
    }
    private void SetWorkHourEntry(Button btn, TextBox txtHours,TextBox id)
    {
        decimal n = decimal.Parse(txtHours.Text);
        _id     =   id.Text;
        if (n.Equals(0))
        {
            txtHours.Text = "1";
            _workhours = (decimal)1;
            btn.BackColor = System.Drawing.Color.LightGray;
            btn.Text = "1.0";
        }

        if (n.Equals((decimal)1))
        {
            txtHours.Text = "0.5";
            _workhours = (decimal)0.5;
            btn.BackColor = System.Drawing.Color.Yellow;
            btn.Text="0.5";
        }

        if (n.Equals((decimal)0.5))
        {
            txtHours.Text = "0";
            _workhours = (decimal)0;
            btn.BackColor = System.Drawing.Color.White;
            btn.Text="0.0";
        }        
    }
    protected void dlstTimeList_ItemDataBound(object sender, DataListItemEventArgs de)
    {       
            DataRowView dv = (DataRowView)de.Item.DataItem;
            TextBox WorkHours = (TextBox)de.Item.FindControl("txtWorkHours");
            TextBox txtid = (TextBox)de.Item.FindControl("txtid");
            Button btn = (Button)de.Item.FindControl("btnTimeEntry");
            Label sno = (Label)de.Item.FindControl("lblsno");
            if (sno != null)
                sno.Text = (de.Item.ItemIndex + 1).ToString();
            sno.Text = de.Item.ItemIndex.ToString().PadLeft(2,'0') + "-" + sno.Text.PadLeft(2, '0');

            WorkHours.Text = dv[_fieldtobind].ToString();
            txtid.Text = dv[_fieldValue].ToString();
            if (btn != null && WorkHours != null)
            {
                SetCurrentWorkHourEntry(btn, WorkHours);
                BindValues(btn, de.Item.ItemIndex);
            }
       
        
    }
    protected void OnTimeStripCommand(EventArgs dce)
    {
        if (TimeStripCommand != null)
            TimeStripCommand(this, dce);
    }
     void bind()
    {

    }
    public void clear()
    {
        dlstTimeList.DataSource = null;
        dlstTimeList.DataBind();
    }
}
