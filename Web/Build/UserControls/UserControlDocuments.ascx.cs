using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class UserControlDocuments : System.Web.UI.UserControl
{
    PhoenixDocumentType documentType;
    private bool _appenddatabounditems;
    public event EventHandler TextChangedEvent;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public PhoenixDocumentType DocumentType
    {
        get { return documentType; }
        set { documentType = value; }
    }

    public DataSet DocumentList
    {
        set
        {
            ddlDocuments.Items.Clear();
            ddlDocuments.DataSource = value;
            ddlDocuments.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlDocuments.Enabled = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlDocuments.CssClass = value;
        }
        get
        {
            return ddlDocuments.CssClass;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlDocuments.AutoPostBack = value;
        }
    }

    public string SelectedDocument
    {
        get
        {

            return ddlDocuments.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDocuments.SelectedIndex = -1;
                ddlDocuments.ClearSelection();
                ddlDocuments.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlDocuments.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDocuments.Items)
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
            return (int.Parse(ddlDocuments.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlDocuments.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDocuments.Items)
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
        ddlDocuments.SelectedIndex = -1;
        switch (documentType)
        {
            case PhoenixDocumentType.COURSE:
                ddlDocuments.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
                break;
            case PhoenixDocumentType.LICENCE:
                ddlDocuments.DataSource = PhoenixRegistersDocumentLicence.ListDocumentLicence(null);
                break;
            case PhoenixDocumentType.MEDICAL:
                ddlDocuments.DataSource = PhoenixRegistersDocumentMedical.ListDocumentMedical();
                break;
            case PhoenixDocumentType.OTHER_DOCUMENT:
                ddlDocuments.DataSource = PhoenixRegistersDocumentOther.ListDocumentOther(null);
                break;
            case PhoenixDocumentType.OTHER_DOC_VISA:
                ddlDocuments.DataSource = PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString());
                break;
            case PhoenixDocumentType.OTHER_DOC_CDC:
                ddlDocuments.DataSource = PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString());
                break;
            case PhoenixDocumentType.OTHER_DOC_NONE:
                ddlDocuments.DataSource = PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString());
                break;
        }
        ddlDocuments.DataBind();
        foreach (RadComboBoxItem item in ddlDocuments.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlDocuments_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlDocuments.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlDocuments_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public Unit Width
    {
        set
        {
            ddlDocuments.Width = value;
        }
        get
        {
            return ddlDocuments.Width;
        }
    }
}
