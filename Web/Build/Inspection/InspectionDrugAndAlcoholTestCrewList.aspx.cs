using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionDrugAndAlcoholTestCrewList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            ViewState["DRUGALCOHOLTESTID"] = "";
            ViewState["COMPLETEDYN"] = "0";

            if (Request.QueryString["DrugAlcoholTestId"] != null)
                ViewState["DRUGALCOHOLTESTID"] = Request.QueryString["DrugAlcoholTestId"].ToString();
            if (Request.QueryString["VesselId"] != null)
                ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();

            
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        SetDetails();

        imgAtt.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
       + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "'); return false;");

        if (ViewState["COMPLETEDYN"].ToString() == "0" || ViewState["COMPLETEDYN"].ToString() == "")
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);
            toolbar1.AddButton("Save Draft", "DRAFT", ToolBarDirection.Right);
            CrewTab.AccessRights = this.ViewState;
            CrewTab.MenuList = toolbar1.Show();
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SetDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void SetDetails()
    {
        try
        {
            DataSet dt = PhoenixInspectionDrugAndAlcoholTest.EditDrugAndAlcoholTest(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(Request.QueryString["DrugAlcoholTestId"].ToString()));

            if (dt.Tables[0].Rows.Count > 0)
            {
                ucDate.Text = dt.Tables[0].Rows[0]["FLDALCOHOLTESTDATE"].ToString();

                if (dt.Tables[0].Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    imgAtt.Controls.Add(html);
                }
                ViewState["attachmentcode"] = dt.Tables[0].Rows[0]["FLDDTKEY"].ToString();

                ViewState["COMPLETEDYN"] = dt.Tables[0].Rows[0]["FLDDRUGALCOHOLTESTYN"].ToString();


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void CrewTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DRAFT"))
            {
                string remarks = ",";
                string alcoholtestcrewid = ",";
                string result = ",";
                string signonoffid = ",";
                string employeeId = ",";
                string employeename = ",";
                string RankId = ",";
                string Rank = ",";

                foreach (GridDataItem item in gvCrewList.Items)
                {
                    alcoholtestcrewid += ((RadLabel)item.FindControl("lblDrugAlcoholTestCrewId")).Text + ",";
                    remarks += (((RadTextBox)item.FindControl("txtSampleTakenEdit")).Text.Trim()) + ",";
                    result += (((RadRadioButtonList)item.FindControl("rblTest")).SelectedValue) + ",";
                    signonoffid += ((RadLabel)item.FindControl("lblsignonoffId")).Text + ",";
                    employeeId += ((RadLabel)item.FindControl("lblCrewId")).Text + ",";
                    employeename += ((RadLabel)item.FindControl("lblCrewName")).Text + ",";
                    RankId += ((RadLabel)item.FindControl("lblRankId")).Text + ",";
                    Rank += ((RadLabel)item.FindControl("lblRank")).Text + ",";

                }

                PhoenixInspectionDrugAndAlcoholTest.DrugAndAlcoholTestCrew(General.GetNullableGuid(ViewState["DRUGALCOHOLTESTID"].ToString())
                                    , alcoholtestcrewid, remarks, result, employeeId, employeename, RankId, Rank, signonoffid);

                ucStatus.Text = "Saved Successfully";

            }

            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
                string remarks = ",";
                string alcoholtestcrewid = ",";
                string result = ",";
                string signonoffid = ",";
                string employeeId = ",";
                string employeename = ",";
                string RankId = ",";
                string Rank = ",";

                foreach (GridDataItem item in gvCrewList.Items)
                {
                    alcoholtestcrewid += ((RadLabel)item.FindControl("lblDrugAlcoholTestCrewId")).Text + ",";
                    remarks += (((RadTextBox)item.FindControl("txtSampleTakenEdit")).Text.Trim()) + ",";
                    result += (((RadRadioButtonList)item.FindControl("rblTest")).SelectedValue) + ",";
                    signonoffid += ((RadLabel)item.FindControl("lblsignonoffId")).Text + ",";
                    employeeId += ((RadLabel)item.FindControl("lblCrewId")).Text + ",";
                    employeename += ((RadLabel)item.FindControl("lblCrewName")).Text + ",";
                    RankId += ((RadLabel)item.FindControl("lblRankId")).Text + ",";
                    Rank += ((RadLabel)item.FindControl("lblRank")).Text + ",";
                }

                PhoenixInspectionDrugAndAlcoholTest.DrugAndAlcoholTestCrewSubmit(General.GetNullableGuid(ViewState["DRUGALCOHOLTESTID"].ToString())
                    , alcoholtestcrewid, remarks, result, employeeId, employeename, RankId, Rank, signonoffid);

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;
            int? vesselid = (Request.QueryString["VesselId"] != null) ? General.GetNullableInteger(Request.QueryString["VesselId"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixInspectionDrugAndAlcoholTest.SearchVesselEmployee(vesselid
                        , General.GetNullableGuid(ViewState["DRUGALCOHOLTESTID"].ToString())
                        , General.GetNullableDateTime(ucDate.Text)
                        , General.GetNullableInteger(ucRank.SelectedRank),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvCrewList.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);

            gvCrewList.DataSource = ds;
            gvCrewList.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

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

    protected void Rebind()
    {
        gvCrewList.SelectedIndexes.Clear();
        gvCrewList.EditIndexes.Clear();
        gvCrewList.DataSource = null;
        gvCrewList.Rebind();
    }
    protected void rblTest_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void gvCrewList_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            string DrugAlcoholTestCrewId = (e.Item as GridDataItem).GetDataKeyValue("FLDDRUGALCOHOLTESTCREWID").ToString();
      
            RadRadioButtonList Test = (RadRadioButtonList)e.Item.FindControl("rblTest");
            if (Test != null)
            {
                Test.SelectedValue = drv["FLDRESULT"].ToString();
         
            }

        }
    }

}
