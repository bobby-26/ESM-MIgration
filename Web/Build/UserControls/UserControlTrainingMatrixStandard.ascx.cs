using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControls_UserControlTrainingMatrixStandard : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _corporate;
    string addresstype = "";
    private int _selectedValue = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlTrainingMatrixStandard.DataSource = PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrixStandardList();
            ddlTrainingMatrixStandard.DataBind();

            foreach (RadComboBoxItem item in ddlTrainingMatrixStandard.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

    public string AddressType
    {
        get
        {
            return addresstype;
        }
        set
        {
            addresstype = value;
        }
    }

    public DataSet AddressList
    {
        set
        {
            ddlTrainingMatrixStandard.Items.Clear();
            ddlTrainingMatrixStandard.DataSource = value;
            ddlTrainingMatrixStandard.DataBind();

            foreach (RadComboBoxItem item in ddlTrainingMatrixStandard.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public bool AppendCorporate
    {
        set
        {
            _corporate = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlTrainingMatrixStandard.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlTrainingMatrixStandard.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlTrainingMatrixStandard.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlTrainingMatrixStandard_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedAddress
    {
        get
        {
            return ddlTrainingMatrixStandard.SelectedValue;
        }
        set
        {
            ddlTrainingMatrixStandard.SelectedIndex = -1;
            if (value.Trim().Equals("") || General.GetNullableInteger(value) == null)
            {
                ddlTrainingMatrixStandard.ClearSelection();
                ddlTrainingMatrixStandard.Text = string.Empty;
                return;
            }
               
            _selectedValue = Int32.Parse(value);

            foreach (RadComboBoxItem item in ddlTrainingMatrixStandard.Items)
            {

                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

    protected void ddlTrainingMatrixStandard_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            if (!_corporate)
                ddlTrainingMatrixStandard.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            else
                ddlTrainingMatrixStandard.Items.Insert(0, new RadComboBoxItem("--Manager--", "Dummy"));
        }
    }

    public string Width
    {
        get
        {
            return ddlTrainingMatrixStandard.Width.ToString();
        }
        set
        {
            ddlTrainingMatrixStandard.Width = Unit.Parse(value);
        }
    }
}
