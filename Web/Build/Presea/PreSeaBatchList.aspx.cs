using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchList : PhoenixBasePage
{
    string InstitudeCode = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MenuBatchList.AccessRights = this.ViewState;
            MenuBatchList.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                BindCourse();
                
                ViewState["BATCHID"] = Request.QueryString["batchid"];
               
                ViewState["BATCHID"] = "";

                if (Session["COURSEID"] != null && Request.QueryString["calledfrom"] != null)
                {

                    ddlCourse.SelectedValue = Session["COURSEID"].ToString();
                    ddlCourse.Enabled = false;                 
                   
                }
                if (Request.QueryString["batchid"] != null)
                {
                    BatchEdit();
                }
                //else
                //{
                //    ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                //        152, "OPN");
                //}

            }
            ddlCourse.Enabled = false;
            DataTable dt = PhoenixPreSeaCommon.GetConfiguration();
            if (dt.Rows.Count > 0)
                InstitudeCode = dt.Rows[0]["FLDREFADDRESSCODE"].ToString();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCourse()
    {
        DataTable dt = PhoenixPreSeaCourse.EditPreSeaCourse(null);
        ddlCourse.DataSource = dt;
        ddlCourse.DataBind();
        ListItem li = new ListItem("--Select--","DUMMY");
        ddlCourse.Items.Insert(0, li);

        if (Request.QueryString["calledfrom"] != null)
        {
            DataRow[] dr = dt.Select("FLDPRESEACOURSEID =" + Request.QueryString["calledfrom"].ToString());
            if (dr.Length > 0)
                Session["COURSEID"] = dr[0]["FLDPRESEACOURSEID"].ToString();
        }
    }



    protected void BatchList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidBatch())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["BATCHID"].ToString() == "")
                {

                    PhoenixPreSeaBatch.InsertBatch(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt64(InstitudeCode)
                    , Convert.ToInt32(ddlCourse.SelectedValue)
                    , Convert.ToDateTime(txtFromDate.Text)
                    , Convert.ToDateTime(txtToDate.Text)
                    , General.GetNullableInteger(null)
                    , General.GetNullableString(txtDuration.Text)
                    , General.GetNullableInteger(null)//chkClosedYN.Checked == true ? "1" : "0"
                    , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    152, "PLN")), 0, General.GetNullableInteger("1"), General.GetNullableInteger("0")
                    , null, null, General.GetNullableByte("1")
                    , General.GetNullableString(txtBatch.Text)
                    , General.GetNullableDecimal(txtRegFees.Text.Trim())
                    , General.GetNullableInteger(txtAGELIMIT.Text.Trim()));

                    string Script = "";
                    Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList(null, 'ifMoreInfo', null);";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

                }
                else
                {

                    PhoenixPreSeaBatch.UpdateBatch(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(ViewState["BATCHID"].ToString())
                    , Convert.ToInt64(InstitudeCode)
                    , Convert.ToInt32(ddlCourse.SelectedValue)
                    , Convert.ToDateTime(txtFromDate.Text)
                    , Convert.ToDateTime(txtToDate.Text)
                    , General.GetNullableInteger(null)
                    , General.GetNullableString(txtDuration.Text)
                    , General.GetNullableInteger(null)//chkClosedYN.Checked == true ? "1" : "0"
                    , General.GetNullableInteger(null)
                    , null
                    , null
                    , General.GetNullableInteger("1")
                    , General.GetNullableInteger("0")
                    , General.GetNullableByte("1")
                    , General.GetNullableString(txtBatch.Text)
                    , General.GetNullableDecimal(txtRegFees.Text.Trim())
                    , General.GetNullableInteger(txtAGELIMIT.Text.Trim()));

                    string Script = "";
                    Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList(null, 'ifMoreInfo', 'yes');";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CalculateDuration(object sender, EventArgs e)
    {

        UserControlDate d = sender as UserControlDate;
        if (d != null)
        {
            if(d.ID.ToUpper()== "TXTFROMDATE")
            {
                txtToDate.Text = (General.GetNullableDateTime(txtFromDate.Text).Value).AddYears(4).ToString();
            }
            if (txtFromDate.Text != null && txtToDate.Text != null)
            {
                DateTime fd = Convert.ToDateTime(txtFromDate.Text);
                DateTime sd = Convert.ToDateTime(txtToDate.Text);
                TimeSpan s = sd - fd;

                txtDuration.Text = Convert.ToString(s.Days + 1);

            }
        }


    }

    private void Reset()
    {
        ViewState["BATCHID"] = "";
        ddlCourse.SelectedValue = "DUMMY";
        txtBatch.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";       
        txtDuration.Text = "";       
        txtRegFees.Text = "";
    }

    protected void BatchEdit()
    {
        try
        {

            int batchid = Convert.ToInt32(Request.QueryString["batchid"]);
            DataSet ds = PhoenixPreSeaBatch.EditBatch(batchid);

            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ddlCourse.SelectedValue = dr["FLDCOURSEID"].ToString();
                txtBatch.Text = dr["FLDBATCH"].ToString();
                txtFromDate.Text = dr["FLDFROMDATE"].ToString();
                txtToDate.Text = dr["FLDTODATE"].ToString();              
                txtDuration.Text = dr["FLDDURATION"].ToString();                
                ViewState["BATCHID"] = dr["FLDBATCHID"].ToString();
                //ucStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                txtRegFees.Text = dr["FLDREGISTERFEES"].ToString();
                txtAGELIMIT.Text = dr["FLDMAXAGELIMIT"].ToString();    

               
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void DisableAll()
    {
        ddlCourse.Enabled = false;
        txtFromDate.Enabled = false;
        txtToDate.Enabled = false;
    }

    private bool IsValidBatch()
    {
        Int32 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";
        if (!Int32.TryParse(ddlCourse.SelectedValue, out resultInt))
            ucError.ErrorMessage = "Course is required";

        if (string.IsNullOrEmpty(txtFromDate.Text))
            ucError.ErrorMessage = "From Date is required.";
        if (string.IsNullOrEmpty(txtToDate.Text))
            ucError.ErrorMessage = "To Date is required.";     
        if (string.IsNullOrEmpty(txtBatch.Text))
            ucError.ErrorMessage = "Batch is required.";
        if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
        {
            ucError.ErrorMessage = "To Date should be greater than From Date";
        }

        return (!ucError.IsError);
    }

}
