using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Collections;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InspectionSealAccept : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionSealAccept.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSeal')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionSealAccept.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionSealAccept.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuSealExport.AccessRights = this.ViewState;
            MenuSealExport.MenuList = toolbar.Show();
           // MenuSealExport.SetTrigger(pnlSealStatus);

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Accept Seals", "ACCEPT",ToolBarDirection.Right);
            MenuSeal.AccessRights = this.ViewState;
            MenuSeal.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucConfirm.Attributes.Add("style", "display:none");
                ucVessel.Enabled = true;
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvSeal.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();
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
        DataSet ds;
        string[] alColumns = { "FLDSEALNO", "FLDVESSELNAME", "FLDSEALTYPENAME", "FLDSTATUSNAME" };
        string[] alCaptions = { "Seal Number", "Vessel", "Seal Type", "Status" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionSealReturn.ReturnedSealSearch(General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableInteger(ucSealType.SelectedQuick)
                , General.GetNullableInteger(ucStatus.SelectedHard)
                , General.GetNullableString(txtFromSealNumber.Text)
                , General.GetNullableString(txtToSealNumber.Text)
                , sortexpression, sortdirection
                , gvSeal.CurrentPageIndex+1
                , gvSeal.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

        General.SetPrintOptions("gvSeal", "Returned Seals", alCaptions, alColumns, ds);
        gvSeal.DataSource = ds;
        gvSeal.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void MenuSeal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ACCEPT"))
            {

                RadWindowManager1.RadConfirm("Are you sure to accept the seals from vessel?", "Confirm", 320, 150, null, "Confirm");
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Are you sure to accept the seals from vessel?";
                //return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            ArrayList SelectedSeals = new ArrayList();
            Guid index = new Guid();
            foreach (GridDataItem row in gvSeal.MasterTableView.Items)
            {

                bool result = false;
                index = new Guid(row.GetDataKeyValue("FLDSEALID").ToString());

                if (((RadCheckBox)(row.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session
                if (Filter.CurrentSelectedSealstoAccept != null)
                    SelectedSeals = (ArrayList)Filter.CurrentSelectedSealstoAccept;
                if (result)
                {
                    if (!SelectedSeals.Contains(index))
                        SelectedSeals.Add(index);
                }
                else
                    SelectedSeals.Remove(index);
            }
            if (SelectedSeals != null && SelectedSeals.Count > 0)
                Filter.CurrentSelectedSealstoAccept = SelectedSeals;

                string s = ",";
                if (Filter.CurrentSelectedSealstoAccept != null)
                {
                    ArrayList selectedseals = (ArrayList)Filter.CurrentSelectedSealstoAccept;
                    if (selectedseals != null && selectedseals.Count > 0)
                    {
                        foreach (Guid index1 in selectedseals)
                        { s = s + index1 + ","; }
                    }
                }
                if (s.Length > 1 && txtFromSealNumber.Text != string.Empty && txtToSealNumber.Text != string.Empty)
                {
                    Filter.CurrentSelectedSealstoAccept = null;
                    BindData();
                    gvSeal.Rebind();
                    //SetPageNavigator();
                    ucError.ErrorMessage = "Do not select the seal(s) from the list when the range is specified.";
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionSealReturn.AcceptSealsFromVessel(General.GetNullableString(s.Length > 1 ? s : null)
                                                        , General.GetNullableString(txtFromSealNumber.Text)
                                                        , General.GetNullableString(txtToSealNumber.Text));

                Filter.CurrentSelectedSealstoAccept = null;
                BindData();
                gvSeal.Rebind();
                //SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSealExport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucSealType.SelectedQuick = "";
                txtFromSealNumber.Text = "";
                txtToSealNumber.Text = "";
                ucStatus.SelectedHard = "";
                ucVessel.SelectedVessel = "";
                BindData();
                gvSeal.Rebind();
               // SetPageNavigator();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvSeal.Rebind();
               // SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSeal_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvSeal_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSeal_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSEALNO", "FLDVESSELNAME", "FLDSEALTYPENAME", "FLDSTATUSNAME" };
        string[] alCaptions = { "Seal Number", "Vessel", "Seal Type", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionSealReturn.ReturnedSealSearch(General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableInteger(ucSealType.SelectedQuick)
                , General.GetNullableInteger(ucStatus.SelectedHard)
                , General.GetNullableString(txtFromSealNumber.Text)
                , General.GetNullableString(txtToSealNumber.Text)
                , sortexpression, sortdirection
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SealStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Returned Seals</h3></td>");
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




    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
        ///SetPageNavigator();
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedSeals = new ArrayList();
        Guid index = new Guid();
        foreach (GridDataItem row in gvSeal.MasterTableView.Items)
        {
            
            bool result = false;
            index = new Guid(gvSeal.MasterTableView.Items[0].GetDataKeyValue("FLDSEALID").ToString());

            if (((RadCheckBox)(row.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Filter.CurrentSelectedSealstoAccept != null)
                SelectedSeals = (ArrayList)Filter.CurrentSelectedSealstoAccept;
            if (result)
            {
                if (!SelectedSeals.Contains(index))
                    SelectedSeals.Add(index);
            }
            else
                SelectedSeals.Remove(index);
        }
        if (SelectedSeals != null && SelectedSeals.Count > 0)
            Filter.CurrentSelectedSealstoAccept = SelectedSeals;
    }

    private void GetSelectedSeals()
    {
        if (Filter.CurrentSelectedSealstoAccept != null)
        {
            ArrayList SelectedSeals = (ArrayList)Filter.CurrentSelectedSealstoAccept;
            Guid index = new Guid();
            if (SelectedSeals != null && SelectedSeals.Count > 0)
            {
                foreach (GridDataItem row in gvSeal.MasterTableView.Items)
                {

                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvSeal.MasterTableView.Items[0].GetDataKeyValue("FLDSEALID").ToString());
                    if (SelectedSeals.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvSeal$ctl00$ctl02$ctl02$chkAllSeal")
        {
            GridHeaderItem headerItem = gvSeal.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllSeal");
            foreach (GridDataItem row in gvSeal.MasterTableView.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                    Filter.CurrentSelectedSealstoAccept = null;
                }
            }
        }
    }

    protected void gvSeal_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
