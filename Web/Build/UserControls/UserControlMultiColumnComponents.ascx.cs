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

public partial class UserControlMultiColumnComponents : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _ItemPerRequest = 50;

    public event EventHandler TextChangedEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
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
    public bool Enabled
    {
        get
        {
            return RadMCComponents.Enabled;
        }
        set
        {
            RadMCComponents.Enabled = value;
        }
    }
    public Unit Width
    {
        get
        {
            return RadMCComponents.Width;
        }
        set
        {
            RadMCComponents.Width = value;
        }
    }
    public string CssClass
    {
        set
        {
            RadMCComponents.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                RadMCComponents.AutoPostBack = true;
        }
    }

    public int selectedvesselid
    {
        get
        {
            return ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        set
        {
            ViewState["VESSELID"] = value;
        }
    }


    public string SelectedValue
    {
        get
        {
            return ViewState[RadMCComponents.ClientID] != null ? ViewState[RadMCComponents.ClientID].ToString() : "";
        }
        set
        {
            ViewState[RadMCComponents.ClientID] = value;
        }

    }
    public string Text
    {
        get
        {
            return RadMCComponents.Text;
        }
        set
        {
            RadMCComponents.Text = value;
        }
    }
    protected void RadMCComponents_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int pageNumber = 1;
        int itemOffset = e.NumberOfItems;
        int count = 0;
        DataSet ds = new DataSet();
        if (e.NumberOfItems > 0)
        {
            pageNumber = ((int)Math.Ceiling((decimal)itemOffset / ItemsPerRequest)) + 1;
        }
        e.Text = e.Text != null ? ((e.Text != string.Empty && e.Text.Contains("-")) ? e.Text.Substring(e.Text.IndexOf("-")+1).Trim() : e.Text) : "";
        
        ds = PhoenixCommonInventory.ComponentSearch(ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID, "","",
               null, null, null, null, null,
               null,
               null, null, null, null,
               pageNumber,
               ItemsPerRequest,
               ref iRowCount,
               ref iTotalPageCount,
               e.Text);

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            RadComboBoxItem Item;
            foreach (DataRow dr in dt.Rows)
            {
                Item = new RadComboBoxItem();

                Item.Text = dr["FLDCOMPONENTNAME"].ToString() != string.Empty ? dr["FLDCOMPONENTNAME"].ToString() : "";
                Item.Value = dr["FLDCOMPONENTID"].ToString();
                RadMCComponents.Items.Add(Item);
            }
        }
        RadMCComponents.DataSource = ds;
        RadMCComponents.DataBind();
        RadMCComponents.SelectedValue = SelectedValue;
        string message = string.Empty;
        if (iRowCount <= 0)
            message = "No matches";
        else
            message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
        e.Message = message;
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void RadMCComponents_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = RadMCComponents.SelectedValue;

        OnTextChangedEvent(e);
    }

    protected void RadMCComponents_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView dr = (DataRowView)e.Item.DataItem;
        e.Item.Text = dr["FLDCOMPONENTNUMBER"] + " - " + e.Item.Text;
    }
}
