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

public partial class UserControlInspectionChapter : System.Web.UI.UserControl
{
    string inspectionid = null;
    int? inspectiontypeid = null;
    int? inspectioncategoryid = null;
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

    public DataSet ChapterList
    {
        set
        {
            ddlInspectionChapter.Items.Clear();
            ddlInspectionChapter.DataSource = value;
            ddlInspectionChapter.DataBind();
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
            ddlInspectionChapter.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlInspectionChapter.Enabled = value;
        }
        get
        {
            return ddlInspectionChapter.Enabled;
        }
    }

    public new bool Visible
    {
        set
        {
            ddlInspectionChapter.Visible = value;
        }
        get
        {
            return ddlInspectionChapter.Visible;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlInspectionChapter.AutoPostBack = true;
        }
    }


    public string SelectedChapter
    {
        get
        {

            return ddlInspectionChapter.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlInspectionChapter.SelectedIndex = -1;
                ddlInspectionChapter.ClearSelection();
                ddlInspectionChapter.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlInspectionChapter.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlInspectionChapter.Items)
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
            return ddlInspectionChapter.SelectedValue;
        }
        set
        {
            _selectedValue = value.ToString();
            ddlInspectionChapter.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlInspectionChapter.Items)
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
        ddlInspectionChapter.DataSource = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableInteger(inspectiontypeid.ToString()),General.GetNullableInteger(inspectioncategoryid.ToString()),General.GetNullableGuid(inspectionid));
        ddlInspectionChapter.DataBind();
        foreach (RadComboBoxItem item in ddlInspectionChapter.Items)
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

    protected void ddlInspectionChapter_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlInspectionChapter_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlInspectionChapter.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        get
        {
            return ddlInspectionChapter.Width.ToString();
        }
        set
        {
            ddlInspectionChapter.Width = Unit.Parse(value);
        }
    }
}
