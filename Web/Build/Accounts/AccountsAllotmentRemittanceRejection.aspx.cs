using SouthNests.Phoenix.Framework;
using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Accounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceRejection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddImageButton("../Accounts/AccountsAllotmentRemittanceRejection.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRemittanceRejection')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsAllotmentRemittanceRejectionFilter.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsAllotmentRemittanceRejection.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuRemittanceRejectionHeaderMain.AccessRights = this.ViewState;
            MenuRemittanceRejectionHeaderMain.MenuList = toolbargrid.Show();
           // MenuRemittanceRejectionHeaderMain.SetTrigger(pnlAllotmentRemittanceRejection);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRemittanceRejection.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentAllotmentRemittanceRejectionFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentAllotmentRemittanceRejectionFilter;
            ds = PhoenixAccountsAllotmentRemittanceRejection.RemittanceRejectionSearch(General.GetNullableString(nvc.Get("txtRemittanceNumberSearch").ToString().Trim())
                            ,General.GetNullableString(nvc.Get("txtFileNumber").ToString().Trim())
                            , General.GetNullableString(nvc.Get("txtName").ToString().Trim())
                            , General.GetNullableInteger(nvc.Get("ddlVessel").ToString().Trim())
                            , General.GetNullableString(nvc.Get("txtFromdateSearch").ToString().Trim())
                            , General.GetNullableString(nvc.Get("txtTodateSearch").ToString().Trim())
                            , General.GetNullableInteger(nvc.Get("chkActiveYN").ToString().Trim())
                            , General.GetNullableInteger(nvc.Get("ddlRejectionReason").ToString().Trim())
                            , sortexpression, sortdirection
                            , (int)ViewState["PAGENUMBER"]
                            , General.ShowRecords(null)
                            , ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixAccountsAllotmentRemittanceRejection.RemittanceRejectionSearch(null, null, null, null, null,null,null,null
                            , sortexpression, sortdirection
                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                            , gvRemittanceRejection.PageSize
                            , ref iRowCount, ref iTotalPageCount);
        }

        string[] alCaptions = { "File No.", "Name", "Vessel", "Remittance No.", "Reason", "Remarks", "Rejected On", "Rejected By" };
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDVESSELNAME", "FLDREMITTANCENUMBER", "FLDREJECTIONREASON", "FLDREMARKS", "FLDREJECTIONDATE", "FLDREJECTEDBY" };

        General.SetPrintOptions("gvRemittanceRejection", "Allotment Remittance Rejection", alCaptions, alColumns, ds);

        gvRemittanceRejection.DataSource = ds;
        gvRemittanceRejection.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "File No.", "Name", "Vessel", "Remittance No.", "Reason", "Remarks", "Rejected On", "Rejected By" };
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDVESSELNAME", "FLDREMITTANCENUMBER", "FLDREJECTIONREASON", "FLDREMARKS", "FLDREJECTIONDATE" , "FLDREJECTEDBY" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentAllotmentRemittanceRejectionFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentAllotmentRemittanceRejectionFilter;
            ds = PhoenixAccountsAllotmentRemittanceRejection.RemittanceRejectionSearch(General.GetNullableString(nvc.Get("txtRemittanceNumberSearch").ToString().Trim())
                                , General.GetNullableString(nvc.Get("txtFileNumber").ToString().Trim())
                                , General.GetNullableString(nvc.Get("txtName").ToString().Trim())
                                , General.GetNullableInteger(nvc.Get("ddlVessel").ToString().Trim())
                                , General.GetNullableString(nvc.Get("txtFromdateSearch").ToString().Trim())
                                , General.GetNullableString(nvc.Get("txtTodateSearch").ToString().Trim())
                                , General.GetNullableInteger(nvc.Get("chkActiveYN").ToString().Trim())
                                , General.GetNullableInteger(nvc.Get("ddlRejectionReason").ToString().Trim())
                                , sortexpression, sortdirection
                                , (int)ViewState["PAGENUMBER"]
                                , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                , ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixAccountsAllotmentRemittanceRejection.RemittanceRejectionSearch(null, null, null, null, null, null, null, null
                            , sortexpression, sortdirection
                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                            , ref iRowCount, ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=AllotmentRemittanceRejection.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Allotment Remittance Rejection</h3></td>");
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

    protected void Rebind()
    {
        gvRemittanceRejection.SelectedIndexes.Clear();
        gvRemittanceRejection.EditIndexes.Clear();
        gvRemittanceRejection.DataSource = null;
        gvRemittanceRejection.Rebind();
    }

    protected void MenuRemittanceRejectionHeaderMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentAllotmentRemittanceRejectionFilter = null;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

  
    protected void gvRemittanceRejection_ItemCommand(object sender, GridCommandEventArgs e)
    {
     
        try
        {
            string RemittanceRejectionId = (e.Item as GridDataItem).GetDataKeyValue("FLDREMITTANCEREJECTIONID").ToString();
            string Remittanceid = ((RadLabel)e.Item.FindControl("lblRemittanceId")).Text;
            string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
            string vesselid = ((RadLabel)e.Item.FindControl("lblvesselid")).Text;
            string vesselname = ((RadLabel)e.Item.FindControl("lblvesselname")).Text;


            if (e.CommandName.ToUpper().Equals("GENERATEPMV"))
            {
                Response.Redirect("AccountsAllotmentRemittanceRejectionGeneratePMV.aspx?EMPLOYEEID=" + employeeid.ToString() + "&VESSELID=" + vesselid.ToString() + "&REMITTANCEID=" + Remittanceid.ToString() + "&VESSELNAME=" + vesselname.ToString(), false);
            }
            //if(e.CommandName.ToUpper().Equals("EARNING"))
            //{
            //    PhoenixAccountsAllotmentRemittanceRejection.AllotmentRemittanceSendEarning(
            //        PhoenixSecurityContext.CurrentSecurityContext.UserCode
            //        ,new Guid(RemittanceRejectionId)
            //        ,int.Parse(employeeid)
            //        ,int.Parse(vesselid)
            //        );

            //    ucStatus.Text = "Earning entry sent to vessel";
            //}
            //  BindData();
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

    protected void gvRemittanceRejection_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

          //  if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
          //      && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                Image lbtn = (Image)e.Item.FindControl("imgToolTip");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTip");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;

            }

         //   if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                //ImageButton db = (ImageButton)e.Row.FindControl("cmdApprove");
                ImageButton db1 = (ImageButton)e.Item.FindControl("cmdRejection");

                ImageButton pmv = (ImageButton)e.Item.FindControl("imgPMVGenerate");

                //ImageButton ear = (ImageButton)e.Row.FindControl("cmdEarning");

                string quickcode = "";
                DataSet ds = PhoenixRegistersQuick.GetQuickCode(162, "PRJ");

                quickcode = ds.Tables[0].Rows[0]["FLDQUICKCODE"].ToString();

                if (drv["FLDREASONID"].ToString().Equals(quickcode))
                {

                    if (pmv != null) pmv.Attributes.Add("style", "visibility:visible");
                    //if (pmv != null) pmv.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to generate new PMV?'); return false;");
                    //if (!SessionUtil.CanAccess(this.ViewState, ear.CommandName)) ear.Visible = true;
                    //if (ear != null) ear.Attributes.Add("style", "visibility:visible");

                    if (db1 != null) db1.Attributes.Add("style", "visibility:hidden");
                }

                if (db1 != null)
                {
                    db1.Attributes.Add("onclick", "javascript:Openpopup('codehelp1', '', '../Accounts/AccountsAllotmentRemittanceRejectionPMV.aspx?remittancerejectionId=" + drv["FLDREMITTANCEREJECTIONID"].ToString() + "&remittanceid=" + drv["FLDREMITTANCEID"].ToString() + "&reasonid=" + drv["FLDREASONID"].ToString() + "');return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
                }
            }
        }

    }

    protected void gvRemittanceRejection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittanceRejection.CurrentPageIndex + 1;
        BindData();
    }
}