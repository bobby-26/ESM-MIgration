using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewHRTravelPassengerSelectionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("Family", "FAMILY",ToolBarDirection.Right);
        MenuFamilyList.AccessRights = this.ViewState;
        MenuFamilyList.MenuList = toolbarmain.Show();
       

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["travelrequestid"] != null)
                ViewState["travelrequestid"] = Request.QueryString["travelrequestid"].ToString();
            else
                ViewState["travelrequestid"] = "";

            if (Request.QueryString["personalinfosn"] != null)
                ViewState["personalinfosn"] = Request.QueryString["personalinfosn"].ToString();
            else
                ViewState["personalinfosn"] = "";

        }
        //BindData();
    }

    protected void FamilyList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EMPLOYEE"))
            {
                Response.Redirect("../Crew/CrewHRTravelPassengerSelectionList.aspx?travelrequestid=" + ViewState["travelrequestid"].ToString() + "&personalinfosn=" + ViewState["personalinfosn"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("FAMILY"))
            {
                Response.Redirect("../Crew/CrewHRTravelPassengerFamilySelectionList.aspx?travelrequestid=" + ViewState["travelrequestid"].ToString() + "&personalinfosn=" + ViewState["personalinfosn"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = new DataTable();

            dt = PhoenixCrewHRTravelRequest.HRFamilySearch(General.GetNullableInteger(ViewState["personalinfosn"].ToString())
                        , null
                       );

            if (dt.Rows.Count > 0)
            {
                gvFamilyList.DataSource = dt;              
            }
            else
            {
                gvFamilyList.DataSource = "";
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void gvFamilyList_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }
    //}

    //protected void gvFamilyList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper() == "ADDPASSENGER")
    //        {
    //            PhoenixCrewHRTravelRequest.HRTravelPassengerInsert(new Guid(ViewState["travelrequestid"].ToString())
    //                , General.GetNullableInteger(ViewState["personalinfosn"].ToString())
    //                , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFamilyMemberId")).Text)
    //                , null
    //                , null
    //                , General.GetNullableString(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFirstName")).Text)
    //                , General.GetNullableString(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMiddleName")).Text)
    //                , General.GetNullableString(((Label)_gridView.Rows[nCurrentRow].FindControl("lblLastName")).Text)
    //                , null
    //                , null
    //                , null
    //                , null
    //                );

    //            ucstatus.Text = "Passenger has been added.";

    //            //string Script = "";
    //            //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //            //Script += " fnReloadList();";
    //            //Script += "</script>" + "\n";
    //            //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    //        }
    //        String script = String.Format("javascript:fnReloadList('code1');");
    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    //    }

    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}
    protected void gvFamilyList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
         
            else if (e.CommandName.ToUpper() == "ADDPASSENGER")
            {
                PhoenixCrewHRTravelRequest.HRTravelPassengerInsert(new Guid(ViewState["travelrequestid"].ToString())
                    , General.GetNullableInteger(ViewState["personalinfosn"].ToString())
                    , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblFamilyMemberId")).Text)
                    , null
                    , null
                    , General.GetNullableString(((RadLabel)e.Item.FindControl("lblFirstName")).Text)
                    , General.GetNullableString(((RadLabel)e.Item.FindControl("lblMiddleName")).Text)
                    , General.GetNullableString(((RadLabel)e.Item.FindControl("lblLastName")).Text)
                    , null
                    , null
                    , null
                    , null
                    );

                ucstatus.Text = "Passenger has been added.";
            }
            String script = String.Format("javascript:fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvFamilyList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    
}
