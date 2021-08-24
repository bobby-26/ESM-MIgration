using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.StandardForm;


public partial class PlannedMaintenanceWorkOrderReportLogHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
            divHistory.Visible = false;
            divHistoryTemplate.Visible = false;
            ViewState["WORKORDERREPORTID"] = "";
            ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            ViewState["FORMURL"] = string.Empty;
            ViewState["DONEID"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            BindFields();
            txtJobDetail.ActiveMode = AjaxControlToolkit.HTMLEditor.ActiveModeType.Preview;
        }
        //if (divHistoryTemplate.Visible)
            //BindData();
    }

    private void BindFields()
    {
        try
        {

            if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.EditWorkOrderReport(new Guid(ViewState["WORKORDERID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtJobDetail.Content = dr["FLDHISTORY"].ToString();
                    ViewState["WORKORDERREPORTID"] = dr["FLDWORKORDERREPORTID"].ToString();
                    ViewState["FORMURL"] = dr["FLDFORMURL"].ToString();
                    ViewState["dtkey"] = dr["FLDDTKEY"].ToString();
                    if (dr["FLDREPORTHISTORY"].ToString() != "1" || dr["FLDFORMURL"].ToString() == string.Empty)
                    {
                        divHistory.Visible = true;
                        divHistoryTemplate.Visible = false;                       
                    }
                    else
                    {
                        divHistory.Visible = false;
                        divHistoryTemplate.Visible = true;
                        //BindData();
                        string script = "parent.resizeFrame('180');";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //private void BindData()
    //{
    //    try
    //    {
    //        int iRowCount = 0;
    //        int iTotalPageCount = 0;

    //        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
    //        int? sortdirection = null;

    //        if (ViewState["SORTDIRECTION"] != null)
    //            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


    //        DataTable dt = PhoenixStandardForm.SearchStandardFormData(General.GetNullableGuid(ViewState["WORKORDERID"].ToString()),
    //            sortexpression, sortdirection,
    //            (int)ViewState["PAGENUMBER"],
    //            General.ShowRecords(null),
    //            ref iRowCount,
    //            ref iTotalPageCount);

    //        if (dt.Rows.Count > 0)
    //        {
    //            gvHistoryTemplate.DataSource = dt;
    //            gvHistoryTemplate.DataBind();
    //            if (ViewState["DONEID"] == null)
    //            {
    //                gvHistoryTemplate.SelectedIndex = 0;
    //                ViewState["DONEID"] = dt.Rows[0]["FLDDONEID"].ToString();
    //                ViewState["FORMURL"] = dt.Rows[0]["FLDFORMURL"].ToString();
    //                ifMoreInfo.Attributes["src"] = "../StandardForm/" + ViewState["FORMURL"].ToString() + "?mode=view&workorderid=" + ViewState["WORKORDERID"].ToString() + "&doneid=" + ViewState["DONEID"].ToString();
    //            }
    //        }
    //        else
    //        {
    //            ifMoreInfo.Attributes["src"] = "../StandardForm/" + ViewState["FORMURL"].ToString() + "?mode=view&workorderid=" + ViewState["WORKORDERID"].ToString();
    //        }

    //        ViewState["ROWCOUNT"] = iRowCount;
    //        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    //        //SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    //protected void gvHistoryTemplate_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.SelectedIndex = se.NewSelectedIndex;
    //    string url = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblFormUrl")).Text;
    //    ViewState["DONEID"] = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblDoneId")).Text;
    //    ViewState["FORMURL"] = url;
    //    BindData();
    //    ifMoreInfo.Attributes["src"] = "../StandardForm/" + ViewState["FORMURL"].ToString() + "?mode=view&workorderid=" + ViewState["WORKORDERID"].ToString() + "&doneid=" + ViewState["DONEID"].ToString();
    //}

  
}
