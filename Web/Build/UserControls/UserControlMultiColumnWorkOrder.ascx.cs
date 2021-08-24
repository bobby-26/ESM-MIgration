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
public partial class UserControlMultiColumnWorkOrder : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _ItemPerRequest = 50;
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
            return RadWorkorder.Enabled;
        }
        set
        {
            RadWorkorder.Enabled = value;
        }
    }
    public Unit Width
    {
        get
        {
            return RadWorkorder.Width;
        }
        set
        {
            RadWorkorder.Width = value;
        }
    }
    public string CssClass
    {
        set
        {
            RadWorkorder.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {
            return ViewState[RadWorkorder.ClientID] != null ? ViewState[RadWorkorder.ClientID].ToString() : "";
        }
        set
        {
            ViewState[RadWorkorder.ClientID] = value;
        }

    }
    public string Text
    {
        get
        {
            return RadWorkorder.Text;
        }
        set
        {
            RadWorkorder.Text = value;
        }
    }
    protected void RadWorkorder_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
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
        ds = PhoenixCommonPlannedMaintenance.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, General.GetNullableGuid("")
            , null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1,
               pageNumber,
               ItemsPerRequest,
               ref iRowCount,
               ref iTotalPageCount);

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            RadComboBoxItem Item;
            foreach (DataRow dr in dt.Rows)
            {
                Item = new RadComboBoxItem();

                Item.Text = dr["FLDTITLE"].ToString();
                Item.Value = dr["FLDWORKORDERID"].ToString();
                RadWorkorder.Items.Add(Item);
            }
        }
        RadWorkorder.DataSource = ds;
        RadWorkorder.DataBind();
        RadWorkorder.SelectedValue = SelectedValue;
        string message = string.Empty;
        if (iRowCount <= 0)
            message = "No matches";
        else
            message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
        e.Message = message;
    }

    protected void RadWorkorder_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = RadWorkorder.SelectedValue;
    }

    protected void RadWorkorder_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView dr = (DataRowView)e.Item.DataItem;
        e.Item.Text = dr["FLDTITLE"] + " - " + e.Item.Text;
    }
}
