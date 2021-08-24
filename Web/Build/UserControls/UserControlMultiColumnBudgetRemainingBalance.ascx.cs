using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
[ValidationPropertyAttribute("SelectedValue")]
public partial class UserControls_UserControlMultiColumnBudgetRemainingBalance : System.Web.UI.UserControl
{
    private int _budgetgroup;
    private int _vesselid;
    private int _hardtypecode;
    private bool _appenddatabounditems;
    private int _ItemPerRequest = 50;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucBudgetGroup.SelectedHard = _budgetgroup.ToString();
            ucBudgetGroup.HardTypeCode = _hardtypecode.ToString();
            //((UserControlHard)RadMCBudget.Header.FindControl("ucBudgetGroup")).HardTypeCode = _hardtypecode.ToString();
        }
        //((UserControlHard)RadMCBudget.FindControl("ucBudgetGroup")).SelectedHard = _budgetgroup.ToString();

    }
    public int ItemsPerRequest
    {
        get
        {
            return _ItemPerRequest;
        }
        set
        {
            _ItemPerRequest = value;
        }
    }
    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public int budgetgroup
    {
        set
        {
            _budgetgroup = value;
        }
    }
    public int vesselid
    {
        set
        {
            _vesselid = value;
        }
    }
    public int hardtypecode
    {
        set
        {
            _hardtypecode = value;
        }
    }
    public bool Enabled
    {
        get
        {
            return RadMCBudget.Enabled;
        }
        set
        {
            RadMCBudget.Enabled = value;
        }
    }
    public Unit Width
    {
        get
        {
            return RadMCBudget.Width;
        }
        set
        {
            RadMCBudget.Width = value;
        }
    }
    public string CssClass
    {
        set
        {
            RadMCBudget.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {
            return ViewState[RadMCBudget.ClientID] != null ? ViewState[RadMCBudget.ClientID].ToString() : "";
        }
        set
        {
            ViewState[RadMCBudget.ClientID] = value;
        }

    }
    public string Text
    {
        get
        {
            return RadMCBudget.Text;
        }
        set
        {
            RadMCBudget.Text = value;
        }
    }
    protected void RadMCBudget_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int pageNumber = 1;
        int itemOffset = e.NumberOfItems;
        int count = 0;

        decimal budgetamount = 0.00M;
        decimal committedamount = 0.00M;
        decimal paidamount = 0.00M;
        decimal variance = 0.00M;

        DataSet ds = new DataSet();
        if (e.NumberOfItems > 0)
        {
            pageNumber = ((int)Math.Ceiling((decimal)itemOffset / ItemsPerRequest)) + 1;
        }
        e.Text = e.Text != null ? ((e.Text != string.Empty && e.Text.Contains("-")) ? e.Text.Substring(e.Text.IndexOf("-") + 1).Trim() : e.Text) : "";
        ds = PhoenixCommonRegisters.BudgetSearch(
            1
            , null
            , null
            , null
            , General.GetNullableInteger(ucBudgetGroup.SelectedHard.ToString())
            //, General.GetNullableInteger(((UserControlHard)RadMCBudget.Header.FindControl("ucBudgetGroup")).SelectedHard.ToString())
            , null  // date
            , General.GetNullableInteger(_vesselid.ToString())// vessel
            , null, null,
            pageNumber,
            ItemsPerRequest,
            ref iRowCount,
            ref iTotalPageCount,
            ref budgetamount,
            ref committedamount,
            ref paidamount,
            ref variance);
        //ds = PhoenixCommonInventory.ComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, "", e.Text,
        //       null, null, null, null, null,
        //       null,
        //       null, null, null, null,
        //       pageNumber,
        //       ItemsPerRequest,
        //       ref iRowCount,
        //       ref iTotalPageCount);

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            RadComboBoxItem Item;
            foreach (DataRow dr in dt.Rows)
            {
                Item = new RadComboBoxItem();

                Item.Text = dr["FLDDESCRIPTION"].ToString() != string.Empty ? dr["FLDDESCRIPTION"].ToString() : "";
                Item.Value = dr["FLDBUDGETID"].ToString();
                RadMCBudget.Items.Add(Item);
            }
        }
        RadMCBudget.DataSource = ds;
        RadMCBudget.DataBind();
        RadMCBudget.SelectedValue = SelectedValue;


        string message = string.Empty;
        if (iRowCount <= 0)
            message = "No matches";
        else
            message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
        e.Message = message;
    }

    protected void RadMCBudget_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = RadMCBudget.SelectedValue;
    }

    protected void RadMCBudget_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView dr = (DataRowView)e.Item.DataItem;
        e.Item.Text = dr["FLDSUBACCOUNT"] + " - " + e.Item.Text;
    }

    protected void ucBudgetGroup_TextChangedEvent(object sender, EventArgs e)
    {
        RadMCBudget.Text = "";
    }
}