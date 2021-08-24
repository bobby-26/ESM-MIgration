using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class UserControlDocumentVaccination : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    public event EventHandler TextChangedEvent;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }

    public DataSet DocumentList
    {
        set
        {
            ddlDocumentVaccination.Items.Clear();
            ddlDocumentVaccination.DataSource = value;
            ddlDocumentVaccination.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlDocumentVaccination.CssClass = value;
        }
        get
        {
            return ddlDocumentVaccination.CssClass;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlDocumentVaccination.AutoPostBack = value;
        }
    }

    public string SelectedDocument
    {
        get
        {
            return ddlDocumentVaccination.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDocumentVaccination.SelectedIndex = -1;
                ddlDocumentVaccination.ClearSelection();
                ddlDocumentVaccination.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlDocumentVaccination.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDocumentVaccination.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public int SelectedValue
    {
        get
        {
            return (int.Parse(ddlDocumentVaccination.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlDocumentVaccination.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDocumentVaccination.Items)
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
        ddlDocumentVaccination.DataSource = PhoenixRegistersDocumentMedical.ListDocumentVaccination();
        ddlDocumentVaccination.DataBind();
        foreach (RadComboBoxItem item in ddlDocumentVaccination.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlDocumentVaccination_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlDocumentVaccination.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlDocumentVaccination_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
}
