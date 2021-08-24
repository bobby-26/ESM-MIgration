using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Data;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewJoiningLettersReports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);

        MenuLettersAndForms.MenuList = toolbarmain.Show();

        NameValueCollection nvc = Filter.CurrentJoiningPaperSelection;

        if (nvc != null)
        {
            ViewState["employeeid"] = nvc.Get("employeeid");
            ViewState["vesselid"] = nvc.Get("vesselid");
            ViewState["rankid"] = nvc.Get("rankid");

            if (nvc.Get("dateofjoining") != null)
                ViewState["dateofjoining"] = nvc.Get("dateofjoining");
            else
                ViewState["dateofjoining"] = "";

            if (nvc.Get("port") != null)
                ViewState["port"] = nvc.Get("port");
            else
                ViewState["port"] = "";

            if (nvc.Get("flightschedule") != null)
            {
                ViewState["flightschedule"] = nvc.Get("flightschedule");
            }
            else
            {
                ViewState["flightschedule"] = "";
            }

            if (nvc.Get("agentaddress") != null)
            {
                ViewState["agentaddress"] = nvc.Get("agentaddress");
            }
            else
            {
                ViewState["agentaddress"] = "";
            }

            if (nvc.Get("seatimeothercomments") != null)
            {
                ViewState["seatimeothercomments"] = nvc.Get("seatimeothercomments");
            }
            else
            {
                ViewState["seatimeothercomments"] = "";
            }

            if (nvc.Get("txtDceAddress") != null)
            {
                ViewState["txtDceAddress"] = nvc.Get("txtDceAddress");
            }
            else
            {
                ViewState["txtDceAddress"] = "";
            }

            if (nvc.Get("cargodetails") != null)
            {
                ViewState["cargodetails"] = nvc.Get("cargodetails");
            }
            else
            {
                ViewState["cargodetails"] = "";
            }


            if (nvc.Get("requesteddce") != null)
                ViewState["requesteddce"] = nvc.Get("requesteddce");
            else
                ViewState["requesteddce"] = "";
        }

        if (!IsPostBack)
        {
            BindGrid();

            ViewState["referenceno"] = "";
        }

    }

    protected void BindGrid()
    {

        DataSet ds = new DataSet();
        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 218, 1, "ITM,ZTR,UCM,NKD,TDN,IML,PCB,PJB,STC");
        gvJoiningPapers.DataSource = ds;
        gvJoiningPapers.DataBind();

        DataSet ds1 = new DataSet();
        ds1 = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 219);
        gvOtherReports.DataSource = ds1;
        gvOtherReports.DataBind();

        DataSet ds2 = new DataSet();
        ds2 = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 220);
        gvSeparateJoining.DataSource = ds2;
        gvSeparateJoining.DataBind();
    }

    protected void MenuLettersAndForms_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Crew/CrewJoiningLetters.aspx", true);
        }

    }

    protected void gvSeparateJoining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {


            string name = ((RadLabel)e.Item.FindControl("lblHardCode")).Text;

            if (name == PhoenixCommonRegisters.GetHardCode(1, 220, "DPT"))
            {
                String scriptpopup = String.Format(
                       "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=DEPARTURE&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

            if (name == PhoenixCommonRegisters.GetHardCode(1, 220, "MBF"))
            {
                if (ViewState["rankid"].ToString().Equals("1") || ViewState["rankid"].ToString().Equals("2"))
                {
                    String scriptpopup = String.Format(
                                "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=MASTERBRIEFING&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else if (ViewState["rankid"].ToString().Equals("6") || ViewState["rankid"].ToString().Equals("7"))
                {
                    String scriptpopup = String.Format(
                               "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CEBRIEFING&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
            }
        }

    }



    public void BindReferenceNo()
    {
        string refno = string.Empty;

        DataTable dt = PhoenixCrewReports.GenerateRefNumber(int.Parse(ViewState["employeeid"].ToString()), int.Parse(ViewState["vesselid"].ToString()), int.Parse(ViewState["rankid"].ToString()), ref refno);

        ViewState["referenceno"] = refno;

    }



    protected void gvOtherReports_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            BindReferenceNo();
            string name = ((RadLabel)e.Item.FindControl("lblShortName")).Text;

            if (name.ToString().ToUpper().Equals("DPT"))
            {
                String scriptpopup = String.Format(
                       "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=DEPARTURE&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

            if (name.ToString().ToUpper().Equals("DLR"))
            {
                String scriptpopup = String.Format(
                        "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=DCELETTER&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&selecteddcetype=" + ViewState["requesteddce"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

            if (name.ToString().ToUpper().Equals("STL"))
            {
                String scriptpopup = String.Format(
                       "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=SEATIME&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
    }


    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                previousRow.Cells[2].RowSpan + 1;
                previousRow.Cells[2].Visible = false;

            }
        }
    }
    protected void BindgvJoiningPapers()
    {
        DataSet ds = new DataSet();
        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 218, 1, "ITM,ZTR,UCM,NKD,TDN,IML,PCB,PJB,STC");
        gvJoiningPapers.DataSource = ds;

    }

    protected void BindgvOtherReports()
    {
        DataSet ds1 = new DataSet();
        ds1 = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 219);
        gvOtherReports.DataSource = ds1;

    }

    protected void BindgvSeparateJoining()
    {
        DataSet ds2 = new DataSet();
        ds2 = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 220);
        gvSeparateJoining.DataSource = ds2;

    }
    protected void gvJoiningPapers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvJoiningPapers.CurrentPageIndex + 1;

        BindgvJoiningPapers();
    }
    protected void gvOtherReports_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOtherReports.CurrentPageIndex + 1;

        BindgvOtherReports();
    }
    protected void gvSeparateJoining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSeparateJoining.CurrentPageIndex + 1;

        BindgvSeparateJoining();
    }
    protected void gvJoiningPapers_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvSeparateJoining_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvOtherReports_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvJoiningPapers_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToString().ToUpper() == "JOIN")
        {

            //BindReferenceNo

            string refno = string.Empty;

            DataTable dt = PhoenixCrewReports.GenerateRefNumber(int.Parse(ViewState["employeeid"].ToString()), int.Parse(ViewState["vesselid"].ToString()), int.Parse(ViewState["rankid"].ToString()), ref refno);

            ViewState["referenceno"] = refno;

            //GetSelectedReports

            string selitems = "";

            int i = 0;
            foreach (GridDataItem row in gvJoiningPapers.Items)
            {
                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkSelection");
                RadLabel lblhardcode = (RadLabel)e.Item.FindControl("lblHardCode");

                if (cb != null && cb.Checked == true)
                {
                    selitems += lblhardcode.Text;
                    selitems += ",";
                }

                i++;
            }
            if (selitems.Length > 0)
            {
                selitems = selitems.Remove(selitems.Length - 1);
                ViewState["reports"] = selitems;
            }
            else
                ViewState["reports"] = "0";

            //Validation

            if (ViewState["reports"].ToString() == "0")
            {
                ucError.ErrorMessage = "Select Atleast One Report";
                ucError.Visible = true;
                return;
            }

            String scriptpopup = String.Format(
                        "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=JOININGLETTERS&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&reports=" + ViewState["reports"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }

    }


}
