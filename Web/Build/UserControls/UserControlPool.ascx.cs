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
public partial class UserControlPool : System.Web.UI.UserControl
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

    public DataSet PoolList
    {
        set
        {
            ddlPool.DataSource = value;
            ddlPool.DataBind();
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
            ddlPool.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {          
                ddlPool.AutoPostBack = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlPool_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedPool
    {
        get
        {
            return ddlPool.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPool.SelectedIndex = -1;
                ddlPool.ClearSelection();
                ddlPool.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlPool.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPool.Items)
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
        ddlPool.DataSource = PhoenixRegistersMiscellaneousPoolMaster.ListMiscellaneousPoolMaster();
        ddlPool.DataBind();
        foreach (RadComboBoxItem item in ddlPool.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public string SelectedPoolName
    {
        get
        {
            return ddlPool.SelectedItem.Text;
        }
    }
    public bool Enabled
    {
        get
        {
            return ddlPool.Enabled;
        }
        set
        {
            ddlPool.Enabled = value;
        }
    }
    protected void ddlPool_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlPool.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        get
        {
            return ddlPool.Width.ToString();
        }
        set
        {
            ddlPool.Width = Unit.Parse(value);
        }
    }
}
