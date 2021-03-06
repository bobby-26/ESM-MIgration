using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventorySpareInOut : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW",ToolBarDirection.Right);
            MenuInventorySpareInOut.AccessRights = this.ViewState;  
            MenuInventorySpareInOut.MenuList = toolbarmain.Show();

            txtComponentName.Attributes.Add("onkeydown", "return false;");
            txtComponentNumber.Attributes.Add("onkeydown", "return false;");
            txtWorkOrderNumber.Attributes.Add("onkeydown", "return false;");
            txtWorkOrderDescription.Attributes.Add("onkeydown", "return false;");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareInOut.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareInOut')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            if (!IsPostBack)
            {
                gvSpareInOut.PageSize = General.ShowRecords(null);
                NameValueCollection nvc = Filter.CurrentSpareItemDispositionHeaderId;

                string dispositionheaderid = string.Empty;

                if (nvc!=null)
                    dispositionheaderid = nvc.Get("DISPOSITIONHEADERID").ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["INTERNALID"] = null;
                txtDispositionDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");

                ddlDispositionType.HardTypeCode = ((int)PhoenixHardTypeCode.TRANSACTIONTYPE).ToString();
                imgShowParentComponent.Attributes.Add("onclick", "return showPickList('spnPickListParentComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx',true);");
            }

            MenuGridSpareInOut.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSpareInOut_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string dispositionheaderid = "";

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "LOCATIONNAME", "FLDDISPOSITIONQUANTITY" };
            string[] alCaptions = { "Number", "Name", "Location", "Quantity" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            if (Filter.CurrentSpareItemDispositionHeaderId != null)
            {
                NameValueCollection nvc = Filter.CurrentSpareItemDispositionHeaderId;
                dispositionheaderid = nvc.Get("DISPOSITIONHEADERID").ToString();

                txtDispositionDate.Text = nvc.Get("DISPOSITIONDATE").ToString();
                ddlDispositionType.SelectedHard = nvc.Get("TRANSACTIONTYPE").ToString();

                txtWorkOrderId.Text = nvc.Get("WORKORDERID").ToString();
                txtWorkOrderNumber.Text = nvc.Get("WORKORDERNUMBER").ToString();
                txtWorkOrderDescription.Text = nvc.Get("WORKORDERDESCRIPTION").ToString();

                txtComponentID.Text = nvc.Get("COMPONENTID").ToString();
                txtComponentNumber.Text = nvc.Get("COMPONENTNUMBER").ToString();
                txtComponentName.Text = nvc.Get("COMPONENTNAME").ToString();

                txtDispositionReference.Text = nvc.Get("REFERENCE").ToString();
            }

            ds = PhoenixInventorySpareItemDisposition.SpareItemDispositionHeaderSearch(dispositionheaderid, PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                   null, null, null, null,
                   sortexpression, sortdirection,
                  gvSpareInOut.CurrentPageIndex + 1,
                  General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvSpareInOut", "Spare Item In Out", alCaptions, alColumns, ds);


            if (!string.IsNullOrEmpty(dispositionheaderid))
            {
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareInOut.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareInOut')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','Inventory/InventorySpareItemInOutTransaction.aspx?componentid=" + txtComponentID.Text + "');return false;", "Add", "<i class=\"fas fa-plus\"></i>", "ADD");
                MenuGridSpareInOut.MenuList = toolbargrid.Show();
            }

            gvSpareInOut.DataSource = ds;
            gvSpareInOut.VirtualItemCount = iRowCount;

           ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuInventorySpareInOut_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            Guid iDisPositionHeaderId = new Guid();
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            NameValueCollection transactionvalues = new NameValueCollection();

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (Filter.CurrentSpareItemDispositionHeaderId == null)
                {
                    if (!IsValidStoreItemDisposition(txtDispositionDate.Text, ddlDispositionType.SelectedHard))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInventorySpareItemDisposition.InsertSpareItemDispositionHeader(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ddlDispositionType.SelectedHard), Convert.ToDateTime(txtDispositionDate.Text),
                        General.GetNullableGuid(txtWorkOrderId.Text), txtComponentID.Text,
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtDispositionReference.Text, ref iDisPositionHeaderId);

                    transactionvalues.Add("DISPOSITIONHEADERID", iDisPositionHeaderId.ToString());

                    transactionvalues.Add("DISPOSITIONDATE", txtDispositionDate.Text);
                    transactionvalues.Add("TRANSACTIONTYPE", ddlDispositionType.SelectedHard);
                    transactionvalues.Add("WORKORDERID", txtWorkOrderId.Text);
                    transactionvalues.Add("WORKORDERNUMBER", txtWorkOrderNumber.Text);
                    transactionvalues.Add("WORKORDERDESCRIPTION", txtWorkOrderDescription.Text);
                    transactionvalues.Add("COMPONENTID", txtComponentID.Text);
                    transactionvalues.Add("COMPONENTNUMBER", txtComponentNumber.Text);
                    transactionvalues.Add("COMPONENTNAME", txtComponentName.Text);
                    transactionvalues.Add("REFERENCE", txtDispositionReference.Text);

                    Filter.CurrentSpareItemDispositionHeaderId = transactionvalues;
                }
                else
                {
                    ucError.ErrorMessage = "Inserted transaction cannot be updated.";
                    ucError.Visible = true;
                    return;
                }

                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareInOut.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareInOut')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','Inventory/InventorySpareItemInOutTransaction.aspx?componentid="+txtComponentID.Text+"');return false;", "Add", "<i class=\"fas fa-plus\"></i>", "ADD");
                MenuGridSpareInOut.MenuList = toolbargrid.Show();
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Filter.CurrentSpareItemDispositionHeaderId = null;

                txtDispositionDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                ddlDispositionType.SelectedHard = "";
                txtWorkOrderId.Text = "";
                txtWorkOrderNumber.Text = "";
                txtWorkOrderDescription.Text = "";
                txtComponentID.Text = "";
                txtComponentNumber.Text = "";
                txtComponentName.Text = "";
                txtDispositionReference.Text = "";
                BindData();
                gvSpareInOut.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvSpareInOut.Rebind();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareInOut.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareInOut')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','Inventory/InventorySpareItemInOutTransaction.aspx?componentid="+txtComponentID.Text+"');return false;", "Add", "<i class=\"fas fa-plus\"></i>", "ADD");
            MenuGridSpareInOut.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidStoreItemDisposition(string transactiondate, string dispositiontype)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;
        DateTime dateresult;
        if (transactiondate.Trim() != "")
        {
            if (DateTime.TryParse(transactiondate, out dateresult) == false)
                ucError.ErrorMessage = "Item transaction date is not in correct format.";
        }

        if (string.IsNullOrEmpty(dispositiontype.Trim()) || dispositiontype.Trim().ToLower() == "dummy")
        {
            if (int.TryParse(dispositiontype, out result) == false)
                ucError.ErrorMessage = "Please select transaction type.";
        }
        return (!ucError.IsError);
    }

    protected void MenuGridSpareInOut_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "LOCATIONNAME", "FLDDISPOSITIONQUANTITY"};
            string[] alCaptions = { "Number", "Name", "Location", "Quantity"};
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvSpareInOut.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string dispositionheaderid = "";

            if (Filter.CurrentSpareItemDispositionHeaderId != null)
            {
                NameValueCollection nvc = Filter.CurrentSpareItemDispositionHeaderId;
                dispositionheaderid = nvc.Get("DISPOSITIONHEADERID").ToString();
            }

            DataSet ds = PhoenixInventorySpareItemDisposition.SpareItemDispositionHeaderSearch(dispositionheaderid,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                null, null, null, null,
                sortexpression, sortdirection,
               1,
               iRowCount, ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=SpareItemDisposition.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Inventory Spare In Out</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSpareInOut_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            ImageButton db = (ImageButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton _doubleClickButton = (LinkButton)item.Cells[0].Controls[0];
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            item.Attributes["ondblclick"] = _jsDouble;
        }
    }

    protected void gvSpareInOut_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        gvSpareInOut.Rebind();

    }

    protected void gvSpareInOut_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void imgWorkorder_Click(object sender, EventArgs e)
    {
        txtWorkOrderNumber.Text = "";
        txtWorkOrderDescription.Text = "";
        txtWorkOrderId.Text = "";
    }

    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        txtComponentName.Text = "";
        txtComponentNumber.Text = "";
        txtComponentID.Text = "";
    }
}
