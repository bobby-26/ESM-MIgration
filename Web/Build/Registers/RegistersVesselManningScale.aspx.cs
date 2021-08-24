using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class RegistersVesselManningScale : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../Registers/RegistersVesselManningScale.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvManningScale')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuRegistersManningScale.AccessRights = this.ViewState;
        MenuRegistersManningScale.MenuList = toolbar1.Show();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);

        toolbar.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
        toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
        toolbar.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            toolbar.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
        toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
        //toolbar.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
        toolbar.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
        toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);

        MenuVesselList.AccessRights = this.ViewState;
        MenuVesselList.MenuList = toolbar.Show();
        MenuVesselList.SelectedMenuIndex = 3;

        if (!IsPostBack)
        {          
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvManningScale.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void Rebind()
    {
        gvManningScale.SelectedIndexes.Clear();
        gvManningScale.EditIndexes.Clear();
        gvManningScale.DataSource = null;
        gvManningScale.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANKNAME", "FLDEQUIVALENTRANKNAME", "FLDOWNERSCALE", "FLDSAFESCALE", "FLDCBASCALE", "FLDCONTRACTPERIOD", "FLDREMARKS" };
        string[] alCaptions = { "Rank Name", "Equivalent Rank", "Owner Scale", "Safe Scale", "CBA Scale", "Contract Period", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselManningScale.ManningScaleSearch(Int16.Parse(Filter.CurrentVesselMasterFilter)
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ManningScale.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Manning Scale</h3></td>");
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

    protected void RegistersManningScale_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDRANKNAME", "FLDEQUIVALENTRANKNAME", "FLDOWNERSCALE", "FLDSAFESCALE", "FLDCBASCALE", "FLDCONTRACTPERIOD", "FLDREMARKS" };
        string[] alCaptions = { "Rank Name", "Equivalent Rank", "Owner Scale", "Safe Scale", "CBA Scale", "Contract Period", "Remarks" };

        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));

        DataRow drVessel = dsVessel.Tables[0].Rows[0];

        txtVessel.Text = drVessel["FLDVESSELNAME"].ToString();
        DataSet ds = PhoenixRegistersVesselManningScale.ManningScaleSearch(Int16.Parse(Filter.CurrentVesselMasterFilter)
            , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           gvManningScale.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvManningScale", "Vessel Manning Scale", alCaptions, alColumns, ds);
        gvManningScale.DataSource = ds;
        gvManningScale.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[1].Rows[0];
            txtOwnerScaleTotal.Text = dr["FLDOWNERSCALETOTAL"].ToString();
            txtSafeScaleTotal.Text = dr["FLDSAFESCALETOTAL"].ToString();
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvManningScale_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvManningScale.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvManningScale_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkRankEdit");
            if (lb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lb.CommandName = "";
            }

            UserControlRank ucRank = (UserControlRank)e.Item.FindControl("ucRank");
            DataRowView drvRank = (DataRowView)e.Item.DataItem;
            if (ucRank != null) ucRank.SelectedRank = drvRank["FLDRANK"].ToString();

            LinkButton anl = (LinkButton)e.Item.FindControl("cmdEquivalentRank");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            if (anl != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                anl.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo', '', '" + Session["sitepath"] + "/Registers/RegistersManningEquivalentRank.aspx?manningscalekey=" + lblDTKey.Text + "'); return false;");
            }
            if (lblDTKey != null)
            {
                LinkButton img = (LinkButton)e.Item.FindControl("imgGroupList");
                if (img != null)
                    img.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo', '', '" + Session["sitepath"] + "/Registers/RegistersManningEquivalentRank.aspx?readonly=1&manningscalekey=" + lblDTKey.Text + "'); return false;");
            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            HtmlGenericControl gc = (HtmlGenericControl)e.Item.FindControl("spnPickListEmployeeAdd");
            LinkButton emp = (LinkButton)e.Item.FindControl("btnShowEmployeeAdd");
            if (emp != null) emp.Attributes.Add("onclick", "showPickList('" + gc.ClientID + "', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListEmployee.aspx', false); return false;");

        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    private void InsertManningScale(string vesselid, string rank, string ownerscale, string safescale, string CBAScale,
        string contractperiod, string remarks)
    {
        if (!IsValidManningScale(rank, ownerscale, safescale, CBAScale, contractperiod))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersVesselManningScale.InsertManningScale(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt16(vesselid)
            , Convert.ToInt16(rank), null, Convert.ToInt16(ownerscale), Convert.ToInt16(safescale), Convert.ToInt16(CBAScale)
            , Convert.ToInt32(contractperiod), remarks);
    }

    private void UpdateManningScale(string vesselid, string manningscaleid, string rank, string ownerscale, string safescale, string CBAScale,
        string contractperiod, string remarks)
    {


        PhoenixRegistersVesselManningScale.UpdateManningScale(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Convert.ToInt16(manningscaleid)
            , Convert.ToInt16(vesselid), Convert.ToInt16(rank)
            , null                                 // Nationality
            , Convert.ToInt16(ownerscale)
            , Convert.ToInt16(safescale)
            , Convert.ToInt16(CBAScale)
            , Convert.ToInt32(contractperiod)
            , remarks);

        ucStatus.Text = "Manning Scale information updated";
    }

    private bool IsValidManningScale(string rank, string ownerscale, string safescale, string CBAscale, string contractperiod)
    {
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(rank, out resultInt))
            ucError.ErrorMessage = "Rank is required.";

        if (!Int16.TryParse(ownerscale, out resultInt))
            ucError.ErrorMessage = "Valid Owner Scale is required.";

        if (!Int16.TryParse(safescale, out resultInt))
            ucError.ErrorMessage = "Safe Scale is required.";

        if (!Int16.TryParse(CBAscale, out resultInt))
            ucError.ErrorMessage = "CBA Scale is required.";

        if (!Int16.TryParse(contractperiod, out resultInt))
            ucError.ErrorMessage = "Contract Period is required.";

        return (!ucError.IsError);
    }

    private void DeleteManningScale(int vesselid, int manningscaleid)
    {
        PhoenixRegistersVesselManningScale.DeleteManningScale(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, manningscaleid, vesselid);
    }
    protected void gvManningScale_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertManningScale(
                   Filter.CurrentVesselMasterFilter
                   , ((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank.ToString()
                   , ((UserControlNumber)e.Item.FindControl("txtOwnerScaleAdd")).Text
                   , ((UserControlNumber)e.Item.FindControl("txtSafeScaleAdd")).Text
                   , ((UserControlNumber)e.Item.FindControl("txtCBAScaleAdd")).Text
                   , ((UserControlNumber)e.Item.FindControl("txtContractPeriodAdd")).Text
                   , ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text);

                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidManningScale(((UserControlRank)e.Item.FindControl("ucRank")).SelectedRank
             , ((UserControlNumber)e.Item.FindControl("txtOwnerScaleEdit")).Text
             , ((UserControlNumber)e.Item.FindControl("txtSafeScaleEdit")).Text
             , ((UserControlNumber)e.Item.FindControl("txtCBAScaleEdit")).Text
             , ((UserControlNumber)e.Item.FindControl("txtContractPeriodEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateManningScale(
                  Filter.CurrentVesselMasterFilter
                  , ((RadLabel)e.Item.FindControl("lblManningScaleIdEdit")).Text
                  , ((UserControlRank)e.Item.FindControl("ucRank")).SelectedRank.ToString()
                  , ((UserControlNumber)e.Item.FindControl("txtOwnerScaleEdit")).Text
                  , ((UserControlNumber)e.Item.FindControl("txtSafeScaleEdit")).Text
                  , ((UserControlNumber)e.Item.FindControl("txtCBAScaleEdit")).Text
                  , ((UserControlNumber)e.Item.FindControl("txtContractPeriodEdit")).Text
                  , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text);

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteManningScale(Int16.Parse(Filter.CurrentVesselMasterFilter), Int32.Parse(((RadLabel)e.Item.FindControl("lblManningScaleId")).Text));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentVesselMasterFilter == null)
        {
            if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                if (Session["NEWMODE"] != null && Session["NEWMODE"].ToString() == "1")
                {
                    Session["NEWMODE"] = 0;
                    //Response.Redirect( "../Registers/RegistersVessel.aspx";
                    return;
                }
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselParticulars.aspx");
            }
            else if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselOfficeAdmin.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMUNICATIONDETAILS"))
            {
                Response.Redirect("../Registers/RegistersVesselCommunicationDetails.aspx");
            }
            else if (CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                Response.Redirect("../Registers/RegisterVesselCertificate.aspx");
            }
            else if (CommandName.ToUpper().Equals("MANNINGSCALE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    Response.Redirect("../Registers/RegistersOffshoreVesselManningScale.aspx");
                else
                    Response.Redirect("../Registers/RegistersVesselManningScale.aspx");
            }
            else if (CommandName.ToUpper().Equals("BUDGET"))
            {
                Response.Redirect("../Registers/RegistersVesselBudget.aspx");
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTSREQUIRED"))
            {
                Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=VESSEL");
            }
            else if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../Registers/RegistersVessel.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselCorrespondence.aspx";
            //}
            //else if (dce.CommandName.ToUpper().Equals("CHATBOX"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter;
            //}
            else if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
            {
                Response.Redirect("../Registers/RegistersVesselFinancialYear.aspx");
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Registers/RegistersVesselHistory.aspx");
            }
            else if (CommandName.ToUpper().Equals("VESSELSEARCH"))
            {
                Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
            }
            else
                Response.Redirect("../Registers/RegistersVesselList.aspx");
        }
    }
}
