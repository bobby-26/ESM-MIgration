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
public partial class CrewOffshoreTrainingMatrixDetails : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //ucConfirm.Visible = false;
            confirm.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Copy", "COPY", ToolBarDirection.Right);
            toolbarsub.AddButton("Edit", "EDIT", ToolBarDirection.Right);
            toolbarsub.AddButton("List", "LIST",ToolBarDirection.Right);
                       
            
            CrewTrainingMenu.AccessRights = this.ViewState;
            CrewTrainingMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["MATRIXID"] = "";
                if (Request.QueryString["matrixid"] != null && Request.QueryString["matrixid"].ToString() != "")
                {
                    ViewState["MATRIXID"] = Request.QueryString["matrixid"].ToString();
                    lblMatrixid.Text = ViewState["MATRIXID"].ToString();
                }
                gvTrainingMatrix.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //if (Request.QueryString["matrixid"] != null && Request.QueryString["matrixid"].ToString() != "")
            //{
            //    BindData();
            //    SetPageNavigator();
            //}
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

            if (CommandName.ToUpper().Equals("EDIT"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingMatrixEdit.aspx?matrixid=" + ViewState["MATRIXID"].ToString());
            }
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingMatrixList.aspx");
            }
            else if (CommandName.ToUpper().Equals("COPY"))
            {
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Are you sure to copy the matrix?";
                //return;

                RadWindowManager1.RadConfirm("Are you sure to copy the matrix?", "confirm", 320, 150, null, "Confirm");
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTYPE", "FLDVALUE", "FLDEXPERIENCE" };
        string[] alCaptions = { "Type", "Value", "Experience" };
        
        try
        {
            DataSet ds = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixDetails(int.Parse(ViewState["MATRIXID"].ToString())
                                                                  , (int)ViewState["PAGENUMBER"], gvTrainingMatrix.PageSize
                                                                  , ref iRowCount, ref iTotalPageCount
                                                              );

            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            DataColumn groupcol = dt.Columns.Add("FLDGROUPBY", typeof(string));
          
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
               
                dt.Rows[i]["FLDGROUPBY"]  = dt.Rows[i]["FLDTYPE"]+":"+ dt.Rows[i]["FLDCAPTION"];
               
            }
            //General.SetPrintOptions("gvTrainingMatrix", "Training and Qualifications Matrix", alCaptions, alColumns, ds);
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

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            //if (ucCM.confirmboxvalue == 1)
            //{
                if (ViewState["MATRIXID"] != null && ViewState["MATRIXID"].ToString() != "")
                {
                    PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixCopy(int.Parse(ViewState["MATRIXID"].ToString()));
                    ucStatus.Text = "Copied Successfully";
                    BindData();
                    gvTrainingMatrix.Rebind();
                }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    //public class GridDecorator
    //{
    //    public static void MergeRows(RadGrid gridView)
    //    {
    //    //    for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
    //    //    {
    //    //        GridDataItem row = gridView.Items[rowIndex];
    //    //        GridDataItem previousRow = gridView.Items[rowIndex + 1];

    //    //        string strCurrentType = ((RadLabel)gridView.Items[rowIndex].FindControl("lblType")).Text;
    //    //        string strCurrentHeading = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHeading")).Text;
    //    //        string strPreviousType = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblType")).Text;
    //    //        string strPreviousHeading = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHeading")).Text;

    //    //        if (strCurrentType == strPreviousType && strCurrentHeading == strPreviousHeading)
    //    //        {
    //    //            row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
    //    //                                   previousRow.Cells[1].RowSpan + 1;
    //    //            previousRow.Cells[1].Visible = false;

    //    //            row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
    //    //                                   previousRow.Cells[1].RowSpan + 1;
    //    //            previousRow.Cells[1].Visible = false;
    //    //        }

    //    //        string strCurrentValue = ((RadLabel)gridView.Items[rowIndex].FindControl("lblValue")).Text;
    //    //        string strPreviousValue = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblValue")).Text;

    //    //        if (strCurrentValue == strPreviousValue && strCurrentHeading == strPreviousHeading)
    //    //        {
    //    //            row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
    //    //                                   previousRow.Cells[1].RowSpan + 1;
    //    //            previousRow.Cells[1].Visible = false;
    //    //        }
    //    //    }
    //    }
    //}

    protected void gvTrainingMatrix_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTrainingMatrix.CurrentPageIndex + 1;
            if (Request.QueryString["matrixid"] != null && Request.QueryString["matrixid"].ToString() != "")
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

    protected void gvTrainingMatrix_PreRender1(object sender, EventArgs e)
    {
       // GridDecorator.MergeRows(gvTrainingMatrix);
    }

    protected void gvTrainingMatrix_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvTrainingMatrix_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
