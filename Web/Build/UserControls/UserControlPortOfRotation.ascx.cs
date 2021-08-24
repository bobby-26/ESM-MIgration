using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;
public partial class UserControlPortOfRotation : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    int? _nationality;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public string Nationality
    {
        get
        {
            return _nationality.ToString();
        }
        set
        {
            _nationality = int.Parse(value);
        }
    }
    public DataSet PortOfRotationList
    {
        set
        {
            ucPortOfRotation.Items.Clear();
            ucPortOfRotation.DataSource = value;
            ucPortOfRotation.DataBind();
        }
    }

    public string Width
    {
        get
        {
            return ucPortOfRotation.Width.ToString();
        }
        set
        {
            ucPortOfRotation.Width = Unit.Parse(value);
        }
    }

    
    
    public string CssClass
    {
        set
        {
            ucPortOfRotation.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucPortOfRotation.AutoPostBack = true;
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

    public string SelectedPortOfRotation
    {
        get
        {

            return ucPortOfRotation.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucPortOfRotation.SelectedIndex = -1;
                ucPortOfRotation.ClearSelection();
                ucPortOfRotation.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ucPortOfRotation.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucPortOfRotation.Items)
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
            return ucPortOfRotation.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ucPortOfRotation.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucPortOfRotation.Items)
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
        ucPortOfRotation.SelectedIndex = -1;
        ucPortOfRotation.DataSource = PhoenixOwnerBudgetRegisters.PortOfRotationList(General.GetNullableInteger(_nationality.ToString()));
        ucPortOfRotation.DataBind();
        foreach (RadComboBoxItem item in ucPortOfRotation.Items)
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

    protected void ucPortOfRotation_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ucPortOfRotation_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucPortOfRotation.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public bool Enabled
    {
        set
        {
            ucPortOfRotation.Enabled = value;
        }
    }
}
