using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersMiscellaneousZoneMaster : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersMiscellaneousZoneMaster.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvZoneMaster')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuRegistersZoneMaster.AccessRights = this.ViewState;
        MenuRegistersZoneMaster.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvZoneMaster.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDZONE", "FLDDESCRIPTION", "FLDEMAIL" };
        string[] alCaptions = { "Zone", "Description", "Email" };
        string sortexpression;
        int sortdirection;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersMiscellaneousZoneMaster.MiscellaneousZoneMasterSearch(null, "", null
            , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MiscellaneousZoneMaster.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Zone</h3></td>");
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

    protected void RegistersRegistersZoneMaster_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void gvZoneMaster_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDZONE", "FLDDESCRIPTION", "FLDEMAIL" };
        string[] alCaptions = { "Zone", "Description", "Email" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        DataSet ds = PhoenixRegistersMiscellaneousZoneMaster.MiscellaneousZoneMasterSearch(null, "", null
            , sortexpression, sortdirection, gvZoneMaster.CurrentPageIndex + 1,
            gvZoneMaster.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvZoneMaster", "Zone", alCaptions, alColumns, ds);
        gvZoneMaster.DataSource = ds;
        gvZoneMaster.VirtualItemCount = iRowCount;

    }
 

    protected void gvZoneMaster_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        GridFooterItem item = (GridFooterItem)gvZoneMaster.MasterTableView.GetItems(GridItemType.Footer)[0];
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            InsertZoneMaster(
            ((RadTextBox)item.FindControl("txtZoneAdd")).Text,
            ((RadTextBox)item.FindControl("txtDescriptionAdd")).Text,
            ((RadTextBox)item.FindControl("txtEmailAdd")).Text);
                gvZoneMaster.Rebind();
        }
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                string ZoneId = ((RadLabel)eeditedItem.FindControl("lblZoneIdEdit")).Text;

                if (ZoneId == string.Empty || ZoneId == "")
                {
                //    InsertZoneMaster(
                //((RadTextBox)eeditedItem.FindControl("txtZoneEdit")).Text,
                //((RadTextBox)eeditedItem.FindControl("txtDescriptionEdit")).Text,
                //((RadTextBox)eeditedItem.FindControl("txtEmailEdit")).Text);
                //    gvZoneMaster.Rebind();
                }
                else
                {
                    string zone = ((RadTextBox)eeditedItem.FindControl("txtZoneEdit")).Text;
                    string desc = ((RadTextBox)eeditedItem.FindControl("txtDescriptionEdit")).Text;
                    string email = ((RadTextBox)eeditedItem.FindControl("txtEmailEdit")).Text;
                    if (!IsValidZone(zone, desc, email))
                    {
                        e.Canceled = true;
                        ucError.Visible = true;
                        return;
                    }
                    UpdateZoneMaster(
                       Int16.Parse(((RadLabel)eeditedItem.FindControl("lblZoneIdEdit")).Text),
                       zone,
                       desc,
                       email);
                    gvZoneMaster.Rebind();
                }

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        
    }
    protected void gvZoneMaster_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

        LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        if (del != null) del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

    }

    protected void gvZoneMaster_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            string RegionId = ((RadLabel)eeditedItem.FindControl("lblZoneId")).Text;

            DeleteZoneMaster(int.Parse(RegionId));
            gvZoneMaster.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvZoneMaster_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
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

    private void InsertZoneMaster(string zone, string description, string email)
    {
        if (!IsValidZone(zone, description, email))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousZoneMaster.InsertMiscellaneousZoneMaster(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, zone, description, email);
    }

    private void UpdateZoneMaster(int zoneid, string zone, string description, string email)
    {
        if (!IsValidZone(zone, description, email))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousZoneMaster.UpdateMiscellaneousZoneMaster(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, zoneid, zone, description, email);
    }

    private void DeleteZoneMaster(int zoneid)
    {
        PhoenixRegistersMiscellaneousZoneMaster.DeleteMiscellaneousZoneMaster(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, zoneid);
    }

    private bool IsValidZone(string Zone, string Description, string Email)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Zone.Trim().Equals(""))
            ucError.ErrorMessage = "Zone is required.";

        if (!string.IsNullOrEmpty(Email) && !General.IsvalidEmail(Email))
            ucError.ErrorMessage = "Email is not valid.";

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
