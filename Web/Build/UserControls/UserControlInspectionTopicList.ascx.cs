using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class UserControlInspectionTopicList : System.Web.UI.UserControl
{
    string chapterid;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstTopic.DataSource = PhoenixInspectionChapterTopic.ListInspectionChapterTopicMultipleSelection(chapterid);
            lstTopic.DataBind();
        }
    }

    public string ChapterId
    {
        get
        {
            return chapterid;
        }
        set
        {
            chapterid = value;
        }
    }

    public string Width
    {
        get
        {
            return lstTopic.Width.ToString();
        }
        set
        {
            lstTopic.Width = Unit.Parse(value);
            chkboxlist.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
    }

    public string SelectedTopic
    {
        get
        {
            StringBuilder strlist = new StringBuilder();

            strlist.Append(",");

            foreach (RadListBoxItem item in lstTopic.Items)
            {
                if (item.Checked == true)
                {
                    if (item.Value != "Dummy")
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

            foreach (RadListBoxItem item in lstTopic.Items)
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

    public DataSet TopicList
    {
        set
        {
            lstTopic.Items.Clear();
            lstTopic.DataSource = value;
            lstTopic.DataBind();
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
            lstTopic.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                lstTopic.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstTopic_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void lstTopic_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            lstTopic.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
    } 
}
