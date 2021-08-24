using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web;
using Telerik.Web.UI;

public partial class CrewOffshoreTrainingMatrixHistoryDetails : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    string lastHeading = String.Empty;
    //    if (gvTrainingMatrix.Controls.Count > 0 && gvTrainingMatrix.Controls[0] != null)
    //    {
    //        Table gridTable = (Table)gvTrainingMatrix.Controls[0];
    //        foreach (GridViewRow gvr in gvTrainingMatrix.Rows)
    //        {
    //            Label lblHeading = (Label)gvr.FindControl("lblHeading");
    //            string heading = "";
    //            if (lblHeading != null) heading = lblHeading.Text;

    //            if (heading != null)
    //            {

    //                if (lastHeading.CompareTo(heading) != 0)
    //                {
    //                    int rowIndex = gridTable.Rows.GetRowIndex(gvr);

    //                    // Add new group header row  

    //                    GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

    //                    TableCell headerCell = new TableCell();

    //                    headerCell.ColumnSpan = gvTrainingMatrix.Columns.Count;

    //                    headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", heading) + "</b></font>";

    //                    headerCell.CssClass = "GroupHeaderRowStyle";

    //                    // Add header Cell to header Row, and header Row to gridTable  

    //                    headerRow.Cells.Add(headerCell);

    //                    gridTable.Controls.AddAt(rowIndex, headerRow);

    //                    // Update lastValue  

    //                    lastHeading = heading;
    //                }
    //            }
    //        }
    //    }
    //    foreach (GridViewRow r in gvTrainingMatrix.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvTrainingMatrix.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }

    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("List", "LIST",ToolBarDirection.Right);
            CrewTrainingMenu.AccessRights = this.ViewState;
            CrewTrainingMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["HISTORYID"] = "";
                ViewState["MATRIXID"] = "";

                if (Request.QueryString["historyid"] != null && Request.QueryString["historyid"].ToString() != "")
                {
                    ViewState["HISTORYID"] = Request.QueryString["historyid"].ToString();
                    lblHistoryid.Text = ViewState["HISTORYID"].ToString();
                }

                if (Request.QueryString["matrixid"] != null && Request.QueryString["matrixid"].ToString() != "")
                {
                    ViewState["MATRIXID"] = Request.QueryString["matrixid"].ToString();
                }

                gvTrainingMatrix.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 
    protected void CrewTrainingMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingMatrixHistoryList.aspx?matrixid=" + ViewState["MATRIXID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTrainingMatrix_RowDataBound(object sender, GridViewRowEventArgs e)
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

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTYPE", "FLDVALUE", "FLDEXPERIENCE" };
        string[] alCaptions = { "Type", "Value", "Experience" };

        try
        {
            DataSet ds = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixHistoryDetails(int.Parse(ViewState["HISTORYID"].ToString())
                                                                  , (int)ViewState["PAGENUMBER"], gvTrainingMatrix.PageSize
                                                                  , ref iRowCount, ref iTotalPageCount
                                                              );

            //General.SetPrintOptions("gvTrainingMatrix", "Training and Qualifications Matrix", alCaptions, alColumns, ds);

            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            DataColumn groupcol = dt.Columns.Add("FLDGROUPBY", typeof(string));

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {

                dt.Rows[i]["FLDGROUPBY"] = dt.Rows[i]["FLDTYPE"] + ":" + dt.Rows[i]["FLDCAPTION"];

            }
            gvTrainingMatrix.DataSource = dt;
            gvTrainingMatrix.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   

   
    protected void gvTrainingMatrix_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTrainingMatrix.CurrentPageIndex + 1;
            if (Request.QueryString["historyid"] != null && Request.QueryString["historyid"].ToString() != "")
            {
                BindData();
              
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTrainingMatrix_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
