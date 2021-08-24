using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;

public partial class PreSeaTraineeRollNoAssignment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar Toolbar = new PhoenixToolbar();
            Toolbar.AddButton("Save", "SAVE");
            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = Toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["CANDIDATEID"] = "";                
                ViewState["BATCH"] = "";                                

                if (Request.QueryString["candidateid"] != null)
                    ViewState["CANDIDATEID"] = Request.QueryString["candidateid"].ToString();
                if (Request.QueryString["batchid"] != null)
                    ViewState["BATCH"] = Request.QueryString["batchid"].ToString();               
                //MenuPreSea.SetTrigger(pnlPreSeaEntranceExam);
                BindRollNo(ddlRollNo);
                SetPrimaryCandidatesDetails();                
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRollNo())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["CANDIDATEID"].ToString() != "" && ViewState["CANDIDATEID"].ToString() != null)
                {
                    PhoenixPreSeaTrainee.PreSeaTraineeBatchRollNoUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            General.GetNullableInteger(ViewState["BATCH"].ToString()),
                            General.GetNullableInteger(ViewState["CANDIDATEID"].ToString()),
                            ddlRollNo.SelectedValue,
                            General.GetNullableInteger(ddlSection.SelectedValue),
                            General.GetNullableInteger(ddlPractical.SelectedValue));

                }
                ucStatus.Text = "Roll No assigned Successfully.";
                string Script = "";
                Script += "<script language='JavaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('RollNo', '');";
                Script += "</script>" + "\n";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "BookMarkScript", Script, false);     
            }
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindRollNo(DropDownList ddl)
    {
        if (General.GetNullableInteger(ViewState["CANDIDATEID"].ToString()).HasValue)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem("--Select--", "");
            ddl.Items.Add(li);
            DataSet ds = PhoenixPreSeaTrainee.PreSeaTraineeBatchRollNoList(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                            General.GetNullableInteger(ViewState["BATCH"].ToString()));
            ddl.DataSource = ds.Tables[0];
            ddl.DataTextField = "FLDROLLNUMBER";
            ddl.DataValueField = "FLDROLLNUMBER";
            ddl.DataBind();
        }
    }

    private void BindData()
    {
        try
        {
         
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    private void SetPrimaryCandidatesDetails()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(ViewState["CANDIDATEID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtName.Text = dr["FLDNAME"].ToString();                
                txtCourse.Text = dr["FLDCOURSENAME"].ToString();
                txtBatchApplied.Text = dr["FLDTRAININGBATCHNAME"].ToString();
                ddlRollNo.SelectedValue = dr["FLDTRBATCHROLLNUMBER"].ToString();
                ddlSection.SelectedValue = dr["FLDTRSECTIONID"].ToString();
                ddlPractical.SelectedValue = dr["FLDTRPRACTICALID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
      public bool IsValidRollNo()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (General.GetNullableString(ddlRollNo.SelectedValue) == null)
            ucError.ErrorMessage = "Select a Batch Roll No.";
       
        return (!ucError.IsError);

    }
}
