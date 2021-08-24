using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewContractDetails : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Contract Information", "CONTRACTINFORMATION");
            toolbar.AddButton("CBA ", "CBACOMPONENT");
            toolbar.AddButton("Standard ", "STANDARDCOMPONENT");
            toolbar.AddButton("Crew Agreed", "CREWAGREECOMPONENT");
            toolbar.AddButton("Contract Letter", "CONTRACT");
            toolbar.AddButton("Back", "LIST");
            MenuCrewContract.AccessRights = this.ViewState;
            MenuCrewContract.MenuList = toolbar.Show();
            MenuCrewContract.SelectedMenuIndex = 4;
            if (!IsPostBack)
                ContactDetails(Request.QueryString["Contractid"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewContract_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("CONTRACTINFORMATION"))
            {
                Response.Redirect("CrewContractPersonal.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            }
            if (dce.CommandName.ToUpper().Equals("CBACOMPONENT"))
            {
                Response.Redirect("CrewContractCBAdetails.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            }
            if (dce.CommandName.ToUpper().Equals("STANDARDCOMPONENT"))
            {
                Response.Redirect("CrewContractStandardComponent.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            }
            if (dce.CommandName.ToUpper().Equals("CREWAGREECOMPONENT"))
            {
                Response.Redirect("CrewContractAgreedComponent.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            }
            if (dce.CommandName.ToUpper().Equals("CONTRACT"))
            {
                Response.Redirect("CrewContractDetails.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            }
            if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                    Response.Redirect("../Crew/CrewContractHistory.aspx?empid=" + Request.QueryString["empid"].ToString(), false);
                else
                    Response.Redirect("../Crew/CrewContractHistory.aspx", false);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ContactDetails(string Contractid)
    {
        DataSet ds = new DataSet();
        ds = PhoenixCrewContract.ReportCrewContractDetails(new Guid(Contractid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            ViewState["EMPLOYEEID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
            txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            txtSeamanBook.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
            txtfileno.Text = dt.Rows[0]["FLDFILENO"].ToString();
            txtrefno.Text = dt.Rows[0]["FLDREFNUMBER"].ToString();
            lblMonthlyAmount.Text = ds.Tables[4].Rows[0]["FLDTOTALMONTHLYAMOUNT"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            gvContract.DataSource = ds.Tables[1];
            gvContract.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[1];
            ShowNoRecordsFound(dt, gvContract);
        }
        if (ds.Tables[2].Rows.Count > 0)
        {
            gvReimbursements.DataSource = ds.Tables[2];
            gvReimbursements.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[2];
            Nil(dt, gvReimbursements);
        }
        if (ds.Tables[3].Rows.Count > 0)
        {
            gvdeduction.DataSource = ds.Tables[3];
            gvdeduction.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[3];
            Nil(dt, gvdeduction);
        }  if (ds.Tables[5].Rows.Count > 0)
        {
            gvsideletter.DataSource = ds.Tables[5];
            gvsideletter.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[5];
            Nil(dt, gvsideletter);
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();
        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }

    private void Nil(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();
        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Black;
        //gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NIL";
    }
}
