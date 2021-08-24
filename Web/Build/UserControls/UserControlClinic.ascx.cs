using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlClinic : System.Web.UI.UserControl
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

	public DataSet ClinicList
	{
		set
		{
			ucClinic.DataSource = value;
			ucClinic.DataBind();
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
			ucClinic.CssClass = value;
		}
	}

    public bool AutoPostBack
    {
        set
        {
            ucClinic.AutoPostBack = value;
        }
    }
    public int? Zone
    {
        get;
        set;
    }
	protected void OnTextChangedEvent(EventArgs e)
	{
		if (TextChangedEvent != null)
			TextChangedEvent(this, e);
	}

	protected void ucClinic_TextChanged(object sender, EventArgs e)
	{
		OnTextChangedEvent(e);
	}

	public string SelectedClinic
	{
		get
		{
			return ucClinic.SelectedValue;
		}
		set
		{
            if (value == string.Empty)
            {
                ucClinic.SelectedIndex = -1;
                ucClinic.ClearSelection();
                ucClinic.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucClinic.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucClinic.Items)
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
        ucClinic.DataSource = PhoenixRegistersClinic.ListClinic(Zone);
        ucClinic.DataBind();
        foreach (RadComboBoxItem item in ucClinic.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

	protected void ucClinic_DataBound(object sender, EventArgs e)
	{
		if (_appenddatabounditems)
			ucClinic.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
	}

    public Unit Width
    {
        get
        {
            return ucClinic.Width;
        }
        set
        {
            ucClinic.Width = value;
        }
    }

}
