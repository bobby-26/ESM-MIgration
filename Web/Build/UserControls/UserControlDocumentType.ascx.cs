using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlDocumentType : System.Web.UI.UserControl
{

    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet DocumentTypeList
    {
        set
        {
            ddlDocumentType.Items.Clear();
            ddlDocumentType.DataSource = value;
            ddlDocumentType.DataBind();
        }
    }
    public string DocumentType
    {
        get;
        set;
    }
	public bool Nokyn
	{

		get;
		set;
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
            ddlDocumentType.CssClass = value;
        }
    }

    public int? Pool
    {
        get;
        set;
    }

    public bool AutoPostBack
    {
        set
        {
            ddlDocumentType.AutoPostBack = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlDocumentType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedDocumentType
    {
        get
        {
            return ddlDocumentType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDocumentType.SelectedIndex = -1;
                ddlDocumentType.ClearSelection();
                ddlDocumentType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlDocumentType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDocumentType.Items)
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
		ddlDocumentType.DataSource = PhoenixRegistersDocumentOther.ListDocumentOther(DocumentType, Nokyn ? byte.Parse("1") : byte.Parse("0"), Pool);
        ddlDocumentType.DataBind();
        foreach (RadComboBoxItem item in ddlDocumentType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlDocumentType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlDocumentType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public Unit Width
    {
        set
        {
            ddlDocumentType.Width = value;
        }
        get
        {
            return ddlDocumentType.Width;
        }
    }
}
