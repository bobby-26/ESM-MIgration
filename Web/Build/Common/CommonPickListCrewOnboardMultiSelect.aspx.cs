using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Collections;
using Telerik.Web.UI;

public partial class CommonPickListCrewOnboardMultiSelect : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
        MenuConfirm.AccessRights = this.ViewState;
        MenuConfirm.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["VesselId"] != null)
                ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();
            if (Session["CHECKED_EMPIDS"] != null) Session.Remove("CHECKED_EMPIDS");
            if (Session["CHECKED_EMPNAME"] != null) Session.Remove("CHECKED_EMPNAME");
            gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        try
        {
            DataSet ds;
            int? vesselid = (Request.QueryString["VesselId"] != null) ? General.GetNullableInteger(Request.QueryString["VesselId"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            DateTime? date = (Request.QueryString["date"] != null) ? General.GetNullableDateTime(Request.QueryString["date"].ToString()) : General.GetNullableDateTime("");
            ds = PhoenixCrewManagement.ListCrewOnboard(
                          vesselid == null ? 0 : vesselid
                          , General.GetNullableInteger(ucRank.SelectedRank)
                          , date
                          , null);
            gvCrewList.DataSource = ds;
            gvCrewList.VirtualItemCount = ds.Tables[0].Rows.Count;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        string Script = "";
    //        NameValueCollection nvc;

    //        if (Request.QueryString["mode"] == "custom")
    //        {

    //            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //            Script += "fnReloadList('codehelp1','ifMoreInfo');";
    //            Script += "</script>" + "\n";

    //            nvc = new NameValueCollection();
    //            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCrew");
    //            nvc.Add(lb.ID, lb.Text.ToString());
    //            Label lbl = (Label)e.Item.FindControl("lblCrewId");
    //            nvc.Add(lbl.ID, lbl.Text);
    //        }
    //        else
    //        {
    //            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
    //            Script += "</script>" + "\n";

    //            nvc = Filter.CurrentPickListSelection;

    //            ArrayList SelectedEmpName = new ArrayList();
    //            string selectedempname = ",";
    //            if (Session["CHECKED_EMPNAME"] != null)
    //            {
    //                SelectedEmpName = (ArrayList)Session["CHECKED_EMPNAME"];
    //                if (SelectedEmpName != null && SelectedEmpName.Count > 0)
    //                {
    //                    foreach (string index in SelectedEmpName)
    //                    { selectedempname = selectedempname + index + ","; }
    //                }
    //            }

    //            if (SelectedEmpName.Count > 0)
    //            {
    //                nvc.Set(nvc.GetKey(1), selectedempname);
    //            }

    //            ArrayList SelectedEmpId = new ArrayList();
    //            string selectedempids = ",";
    //            if (Session["CHECKED_EMPIDS"] != null)
    //            {
    //                SelectedEmpId = (ArrayList)Session["CHECKED_EMPIDS"];
    //                if (SelectedEmpId != null && SelectedEmpId.Count > 0)
    //                {
    //                    foreach (int indexn in SelectedEmpId)
    //                    { selectedempids = selectedempids + indexn + ","; }
    //                }
    //            }

    //            if (SelectedEmpId.Count > 0)
    //            {
    //                nvc.Set(nvc.GetKey(2), selectedempids);
    //            }
    //            else
    //            {
    //                ucError.ErrorMessage = "Please select Employee.";
    //                ucError.Visible = true;
    //                return;
    //            }
    //        }
    //        Filter.CurrentPickListSelection = nvc;
    //        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void Confirm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                string Script = "";
                NameValueCollection nvc;

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                ArrayList SelectedEmpName = new ArrayList();
                string selectedempname = ",";
                if (Session["CHECKED_EMPNAME"] != null)
                {
                    SelectedEmpName = (ArrayList)Session["CHECKED_EMPNAME"];
                    if (SelectedEmpName != null && SelectedEmpName.Count > 0)
                    {
                        foreach (string index in SelectedEmpName)
                        { selectedempname = selectedempname + index + ","; }
                    }
                }

                if (SelectedEmpName.Count > 0)
                {
                    nvc.Set(nvc.GetKey(1), selectedempname);
                }

                ArrayList SelectedEmpId = new ArrayList();
                string selectedempids = ",";
                if (Session["CHECKED_EMPIDS"] != null)
                {
                    SelectedEmpId = (ArrayList)Session["CHECKED_EMPIDS"];
                    if (SelectedEmpId != null && SelectedEmpId.Count > 0)
                    {
                        foreach (int indexn in SelectedEmpId)
                        { selectedempids = selectedempids + indexn + ","; }
                    }
                }

                if (SelectedEmpId.Count > 0)
                {
                    nvc.Set(nvc.GetKey(2), selectedempids);
                }
                else
                {
                    ucError.ErrorMessage = "Please select Employee.";
                    ucError.Visible = true;
                    return;
                }

                Filter.CurrentPickListSelection = nvc;
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedEmpId = new ArrayList();
        ArrayList SelectedEmpName = new ArrayList();
        int index;
        foreach (GridDataItem gvrow in gvCrewList.Items)
        {
            bool result = false;

            RadLabel lblCrewId = (RadLabel)gvrow.FindControl("lblCrewId");
            index = Convert.ToInt32(lblCrewId.Text);
            LinkButton lnkCrew = (LinkButton)gvrow.FindControl("lnkCrew");
            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;
            }
            // Check in the Session
            if (Session["CHECKED_EMPIDS"] != null)
                SelectedEmpId = (ArrayList)Session["CHECKED_EMPIDS"];
            if (result)
            {
                if (!SelectedEmpId.Contains(index))
                    SelectedEmpId.Add(index);
            }
            else
                SelectedEmpId.Remove(index);

            if (SelectedEmpId != null && SelectedEmpId.Count > 0)
                Session["CHECKED_EMPIDS"] = SelectedEmpId;


            if (Session["CHECKED_EMPNAME"] != null)
                SelectedEmpName = (ArrayList)Session["CHECKED_EMPNAME"];
            if (result)
            {
                if (!SelectedEmpName.Contains(lnkCrew.Text))
                    SelectedEmpName.Add(lnkCrew.Text);
            }
            else
                SelectedEmpName.Remove(lnkCrew.Text);
        }
        if (SelectedEmpName != null && SelectedEmpName.Count > 0)
            Session["CHECKED_EMPNAME"] = SelectedEmpName;
    }

    protected void CheckAll(Object sender, EventArgs e)
     {
      
        string[] ctl = Request.Form.GetValues("__EVENTTARGET"); 
        if (ctl != null && ctl[0].ToString() == "gvCrewList$ctl00$ctl02$ctl01$chkAllEmployee")
        {
            RadCheckBox chkAll =null;
            foreach (GridHeaderItem headerItem in gvCrewList.MasterTableView.GetItems(GridItemType.Header))
            {
                 chkAll = (RadCheckBox)headerItem.FindControl("chkAllEmployee");
               
            }
            
            foreach (GridDataItem row in gvCrewList.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (cbSelected != null )
                {
                    if (chkAll.Checked == true && chkAll != null) cbSelected.Checked = true;
                    else cbSelected.Checked = false;
                }
            }
        }
    }

    private void GetSelectedEmp()
    {
        if (Session["CHECKED_EMPIDS"] != null)
        {
            ArrayList SelectedEmpId = (ArrayList)Session["CHECKED_EMPIDS"];
            int index;
            if (SelectedEmpId != null && SelectedEmpId.Count > 0)
            {
                foreach (GridDataItem row in gvCrewList.Items)
                {                    
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    RadLabel lblCrewId = (RadLabel)row.FindControl("lblCrewId");
                    index = Convert.ToInt32(lblCrewId.Text);
                    if (SelectedEmpId.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }

    }
}
