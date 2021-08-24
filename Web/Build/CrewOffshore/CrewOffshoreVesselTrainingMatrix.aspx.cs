using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreVesselTrainingMatrix : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();

     
        toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreVesselTrainingMatrix.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
       
        toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreVesselTrainingMatrix.aspx", "Edit", "<i class=\"fas fa-table-72\"></i>", "EDIT");

        CrewTrainingMenu.MenuList = toolbarsub.Show();
        if (!IsPostBack)
        {
            confirm.Attributes.Add("style", "display:none");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucVessel.Enabled = false;
            }
            else
            {
                if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                    ucVessel.SelectedVessel = ViewState["VESSELID"].ToString();
            }
        }
    }

    protected void gvTrainingMatrix_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvTrainingMatrix_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTYPE", "FLDVALUE", "FLDEXPERIENCE" };
        string[] alCaptions = { "Type", "Value", "Experience" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = new DataTable();
            if (ucRank.SelectedRank !="")
            {
                DataSet ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixVesselTypeDetailsList(General.GetNullableInteger(ucVessel.SelectedValue.ToString())
                                                                           , General.GetNullableInteger(ucRank.SelectedValue.ToString()));


               
                dt = ds.Tables[0];
                DataColumn groupcol = dt.Columns.Add("FLDGROUPBY", typeof(string));

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    dt.Rows[i]["FLDGROUPBY"] = dt.Rows[i]["FLDTYPE"] + ":" + dt.Rows[i]["FLDCAPTION"];

                }
               
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

    protected void ucRank_TextChangedEvent(object sender, EventArgs e)
    {
        BindData();
        gvTrainingMatrix.Rebind();
    }

    protected void confirm_Click(object sender, EventArgs e)
    {

    }

    protected void CrewTrainingMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvTrainingMatrix.Rebind();
        }
        if (CommandName.ToUpper().Equals("EDIT"))
        {
            if (ucVessel.SelectedVessel != "" && ucRank.SelectedRank != "")
                Response.Redirect("../CrewOffshore/CrewOffshoreSTCWTrainingMatrix.aspx?VESSELTYPE=" + ucVessel.SelectedValue.ToString() + "&FORMTYPE=" + ucRank.SelectedRank.ToString() + "");
        }
    }

    protected void gvTrainingMatrix_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}