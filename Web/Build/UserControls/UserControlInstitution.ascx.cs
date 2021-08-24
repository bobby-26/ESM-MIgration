using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlInstitution : System.Web.UI.UserControl
{

    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
	string courseid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet InstitutionList
    {
        set
        {
            ddlInstitution.DataSource = value;
            ddlInstitution.DataBind();
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

	public bool Enabled
	{
		set
		{
			ddlInstitution.Enabled = value;
		}
	}
	public string Course
	{
		get
		{
			return courseid.ToString();
		}
		set
		{
			courseid = value;
		}
	}
    public string CssClass
    {
        set
        {
            ddlInstitution.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlInstitution.AutoPostBack = true;
        }
    }

    public int Width
    {
        set
        {
            ddlInstitution.Width = value;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlInstitution_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedInstitution
    {
        get
        {
            return ddlInstitution.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlInstitution.SelectedIndex = -1;
                ddlInstitution.ClearSelection();
                ddlInstitution.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlInstitution.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlInstitution.Items)
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
        ddlInstitution.DataSource = PhoenixRegistersInstitution.ListInstitution(General.GetNullableInteger(courseid));
        ddlInstitution.DataBind();
        foreach (RadComboBoxItem item in ddlInstitution.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlInstitution_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlInstitution.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
