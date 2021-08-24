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
public partial class UserControlInspectionTopic : System.Web.UI.UserControl
{
    string inspectionid = null;
    string chapterid = null;
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

    public DataSet TopicList
    {
        set
        {
            ddlInspectionTopic.Items.Clear();
            ddlInspectionTopic.DataSource = value;
            ddlInspectionTopic.DataBind();
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
            ddlInspectionTopic.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlInspectionTopic.Enabled = value;
        }
        get
        {
            return ddlInspectionTopic.Enabled;
        }
    }

    public new bool Visible
    {
        set
        {
            ddlInspectionTopic.Visible = value;
        }
        get
        {
            return ddlInspectionTopic.Visible;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlInspectionTopic.AutoPostBack = true;
        }
    }

    public string SelectedTopic
    {
        get
        {

            return ddlInspectionTopic.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlInspectionTopic.SelectedIndex = -1;
                ddlInspectionTopic.ClearSelection();
                ddlInspectionTopic.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlInspectionTopic.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlInspectionTopic.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddlInspectionTopic.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddlInspectionTopic.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlInspectionTopic.Items)
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
        ddlInspectionTopic.DataSource = PhoenixInspectionChapterTopic.ListInspectionChapterTopic(General.GetNullableGuid(inspectionid), General.GetNullableGuid(chapterid));
        ddlInspectionTopic.DataBind();
        foreach (RadComboBoxItem item in ddlInspectionTopic.Items)
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

    protected void ddlInspectionTopic_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlInspectionTopic_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlInspectionTopic.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        get
        {
            return ddlInspectionTopic.Width.ToString();
        }
        set
        {
            ddlInspectionTopic.Width = Unit.Parse(value);
        }
    }
}
