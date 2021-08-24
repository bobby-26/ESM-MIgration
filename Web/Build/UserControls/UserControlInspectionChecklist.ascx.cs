using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class UserControlInspectionChecklist : System.Web.UI.UserControl
{
    string inspectionid = null;
    string chapterid = null;
    string topicid = null;
    int? sectionid = null;

    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
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

    public string ChapterId
    {
        get
        {
            return chapterid.ToString();
        }
        set
        {
            chapterid = value.ToString();
        }
    }

    public string TopicId
    {
        get
        {
            return topicid.ToString();
        }
        set
        {
            topicid = value.ToString();
        }
    }

    public string SectionId
    {
        get
        {
            return sectionid.ToString();
        }
        set
        {
            sectionid = Int32.Parse(value);
        }
    }

    public DataSet CheckListList
    {
        set
        {
            ddlInspectionChecklist.Items.Clear();
            ddlInspectionChecklist.DataSource = value;
            ddlInspectionChecklist.DataBind();
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
            ddlInspectionChecklist.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlInspectionChecklist.AutoPostBack = true;
        }
    }


    public string SelectedValue
    {
        get
        {

            return ddlInspectionChecklist.SelectedValue;
        }
        set
        {
            _selectedValue = value.ToString();
            ddlInspectionChecklist.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlInspectionChecklist.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedChecklist
    {
        get
        {
            return ddlInspectionChecklist.SelectedValue;
        }
        set
        {
            if (value.ToString() == string.Empty)
            {
                ddlInspectionChecklist.SelectedIndex = -1;
                ddlInspectionChecklist.ClearSelection();
                ddlInspectionChecklist.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlInspectionChecklist.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlInspectionChecklist.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    private void bind()
    {
        ddlInspectionChecklist.DataSource = PhoenixInspectionChecklist.ListInspectionChecklist(General.GetNullableGuid(inspectionid), General.GetNullableGuid(chapterid), General.GetNullableGuid(topicid),General.GetNullableInteger(sectionid.ToString()));
        ddlInspectionChecklist.DataBind();
        foreach (RadComboBoxItem item in ddlInspectionChecklist.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlInspectionChecklist_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlInspectionChecklist_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlInspectionChecklist.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
