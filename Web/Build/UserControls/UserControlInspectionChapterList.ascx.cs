using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class UserControlInspectionChapterList : System.Web.UI.UserControl
{
    string inspectionid = null;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstChapter.DataSource = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableGuid(inspectionid));
            lstChapter.DataBind();
        }
    }
    
    public string InspectionId
    {
        get
        {
            return inspectionid.ToString();
        }
        set
        {
            inspectionid = value.ToString();
        }
    }

    public string SelectedChapter
    {
        get
        {
            StringBuilder strlist = new StringBuilder();

            strlist.Append(",");

            foreach (RadListBoxItem item in lstChapter.Items)
            {
                if (item.Checked == true)
                {
                    if (item.Value.ToString() != "Dummy")
                    {
                        strlist.Append(item.Value.ToString());
                        strlist.Append(",");
                    }
                }
            }
            if (strlist.Length == 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            return strlist.ToString();
        }
        set
        {
            StringBuilder strlist = new StringBuilder();

            foreach (RadListBoxItem item in lstChapter.Items)
            {
                if (item.Checked == true)
                {
                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }
            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            value = strlist.ToString();
        }
    }

    public string Width
    {
        get
        {
            return lstChapter.Width.ToString();
        }
        set
        {
            lstChapter.Width = Unit.Parse(value);
            chkboxlist.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
    }

    public DataSet ChapterList
    {
        set
        {
            lstChapter.Items.Clear();
            lstChapter.DataSource = value;
            lstChapter.DataBind();
        }
    }

    public string AppendDataBoundItems
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _appenddatabounditems = true;
            else
                _appenddatabounditems = false;
        }
    }

    public string CssClass
    {
        set
        {
            chkboxlist.Attributes["class"] = value;
            lstChapter.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                lstChapter.AutoPostBack = true;
        }
    }
    
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstChapter_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void lstChapter_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            lstChapter.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
    } 
}
