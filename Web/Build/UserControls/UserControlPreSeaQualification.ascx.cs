using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlPreSeaQualification : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _csvqualifications;
    private string _selectedqualificationname;
    public event EventHandler TextChangedEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bind();
        }
    }

    public DataSet QualificationList
    {
        set
        {
            ddlPreSeaQualification.DataSource = value;
            ddlPreSeaQualification.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlPreSeaQualification.CssClass = value;
        }
    }

    public string CSVQualification
    {
        set
        {
            _csvqualifications = value;
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

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlPreSeaQualification.AutoPostBack = true;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlPreSeaQualification.Enabled = value;
        }
    }
    public string SelectedQualification
    {
        get
        {
            return ddlPreSeaQualification.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPreSeaQualification.SelectedIndex = -1;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlPreSeaQualification.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPreSeaQualification.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedQualificationName
    {
        get
        {
            return ddlPreSeaQualification.SelectedItem.Text;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPreSeaQualification.SelectedIndex = -1;
                ddlPreSeaQualification.ClearSelection();
                ddlPreSeaQualification.Text = string.Empty;
                return;
            }
            _selectedqualificationname = value;
            ddlPreSeaQualification.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPreSeaQualification.Items)
            {
                if (item.Value == _selectedqualificationname.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public void bind()
    {
        if(String.IsNullOrEmpty(_csvqualifications))
            ddlPreSeaQualification.DataSource = PhoenixRegistersDocumentQualification.ListDocumentPreSeaQualification();
        else
            ddlPreSeaQualification.DataSource = PhoenixRegistersDocumentQualification.ListQualifications(_csvqualifications);

        ddlPreSeaQualification.DataBind();
        foreach (RadComboBoxItem item in ddlPreSeaQualification.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlPreSeaQualification_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlPreSeaQualification.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlPreSeaQualification_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Write(ddlPreSeaQualification.SelectedItem.ToString());
    }
    protected void ddlPreSeaQualification_OnTextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
}
