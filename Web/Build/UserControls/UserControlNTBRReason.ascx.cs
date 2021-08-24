using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlNTBRReason : System.Web.UI.UserControl
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

    public DataSet NTBRReasonList
    {
        set
        {
            uclNTBRReason.DataSource = value;
            uclNTBRReason.DataBind();
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
            uclNTBRReason.CssClass = value;
        }
    }

    public string Width
    {
        set
        {
            uclNTBRReason.Width = Unit.Parse(value);
        }
    }

    public bool Readonly
    {
        get
        {
            return uclNTBRReason.Enabled;
        }
        set
        {
            uclNTBRReason.Enabled = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                uclNTBRReason.AutoPostBack = true;
        }
    }
	public bool Enabled
	{
		get
		{
			return uclNTBRReason.Enabled;
		}
		set
		{
			uclNTBRReason.Enabled = value;
		}
	}
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void uclNTBRReason_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedNTBRMgrReason
    {
        get
        {
            return uclNTBRReason.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                uclNTBRReason.SelectedIndex = -1;
                uclNTBRReason.ClearSelection();
                uclNTBRReason.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            uclNTBRReason.SelectedIndex = -1;
            foreach (RadComboBoxItem item in uclNTBRReason.Items)
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
        uclNTBRReason.DataSource = PhoenixRegistersreasonsntbr.Listreasonsntbr();
        uclNTBRReason.DataBind();
        foreach (RadComboBoxItem item in uclNTBRReason.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    public string SelectedNTBRMgrReasonText
    {
        get
        {
            return uclNTBRReason.SelectedItem.Text;
        }
    }

    protected void uclNTBRReason_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            uclNTBRReason.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
