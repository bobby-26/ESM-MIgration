using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Log_ElectricLogItemTransfer : PhoenixBasePage
{
    public event EventHandler TextChangedEvent;
    string ReportCode;
    Guid ReportId;
    int usercode = 0;
    int vesselId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        string Code = Request.QueryString["Code"];
        string ItemNo = Request.QueryString["ItemNo"];
        string Item = Request.QueryString["Item"];
        string Location = Request.QueryString["Location"];
        ReportCode = Request.QueryString["ReportCode"];
        ReportId = new Guid(Request.QueryString["ReportId"]);
        txtCode.Text = Code;
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuSludgeTransfer.MenuList = toolbar.Show();
        if (IsPostBack == false)
        {
            BindLocation();
            txtOperationDate.Text = (DateTime.Now).ToShortDateString();
            int index = ddlTransferFrom.FindItemIndexByText(Location);
            ddlTransferFrom.SelectedIndex = index;
            ddlFromROB();
            ddlTOROB();
            ViewState["otherInfo"] = false;
            ViewState["TransferTo"] = string.Empty;
            ViewState["RobTo"] = 0;
            ShowHideControlsBasedOnReport();
        }
        if (IsPostBack)
        {
            var ctrlName = Request.Params[Page.postEventSourceID];
            var args = Request.Params[Page.postEventArgumentID];
            HandleCustomPostbackEvent(ctrlName, args);
        }
    }

    private void ShowHideControlsBasedOnReport()
    {
        if (String.IsNullOrWhiteSpace(ReportCode)) return;
        switch (ReportCode)
        {
            case "IC":
                lblTo.Visible = false;
                ddlTransferTo.Visible = false;
                lblBfrTrnsROBTo.Visible = false;
                txtBfrTrnsROBTo.Visible = false;
                lbltounit.Visible = false;
                lblAfrTrnsROBTo.Visible = false;
                txtAfrTrnsROBTo.Visible = false;
                lblaftrtoUnit.Visible = false;
                lblStartTime.Visible = false;
                startTime.Visible = false;
                lblEndTime.Visible = false;
                endTime.Visible = false;
                lblOtherInfo1.Visible = true;
                txtOtherInfo1.Visible = true;
                lblOtherInfo2.Visible = true;
                txtOtherInfo2.Visible = true;
                lblOtherInfo1.Text = "Incineration Time in Hours";
                txtOtherInfo1.InputType = Html5InputType.Number;
                txtOtherInfo2.InputType = Html5InputType.Number;
                lblOtherInfo2.Text = "Incineration Ltrs/Hour";
                ViewState["otherInfo"] = true;
                ViewState["TransferToText"] = "Burned";
                ViewState["TransferToValue"] =  "BUR";
                ViewState["RobTo"] = 0;
                break;
            case "":

                break;
            default:
                lblOtherInfo1.Visible = false;
                txtOtherInfo1.Visible = false;
                lblOtherInfo2.Visible = false;
                txtOtherInfo2.Visible = false;
                lblStartTime.Visible = false;
                startTime.Visible = false;
                lblEndTime.Visible = false;
                endTime.Visible = false;
                break;
        }
    }

    private void HandleCustomPostbackEvent(string ctrlName, string args)
    {
        if (txtBfrTrnsROBFrom.Text != "" && txtAfrTrnsROBFrom.Text != "" && txtBfrTrnsROBTo.Text != "")
        {
            txtAfrTrnsROBTo.Text = ((Convert.ToInt32(txtBfrTrnsROBFrom.Text) - Convert.ToInt32(txtAfrTrnsROBFrom.Text)) + Convert.ToInt32(txtBfrTrnsROBTo.Text)).ToString();
        }
    }

    protected void MenuSludgeTransfer_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //if (!isValidInput())
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                string TransferToSelectedText;
                string TransferToSelectedValue;
                string robTo = "0";
                if (ViewState["otherInfo"] != null && Convert.ToBoolean(ViewState["otherInfo"]) == true)
                {
                    TransferToSelectedText = ViewState["TransferToText"].ToString();
                    TransferToSelectedValue = ViewState["TransferToValue"].ToString();
                    robTo = ViewState["RobTo"].ToString();
                }
                else
                {
                    TransferToSelectedText = ddlTransferTo.SelectedItem.Text;
                    TransferToSelectedValue = ddlTransferTo.SelectedItem.Value;
                    robTo = txtBfrTrnsROBTo.Text;
                }
                
                Guid TranscationId = Guid.Empty;
                 int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                //PhoenixElog.InsertTransaction(      usercode
                //                                , ReportCode
                //                                , ddlTransferFrom.SelectedItem.Value
                //                                , ddlTransferFrom.SelectedItem.Text
                //                                , TransferToSelectedValue
                //                                , TransferToSelectedText
                //                                , txtCode.Text
                //                                , Request.QueryString["ItemNo"].ToString()
                //                                , Request.QueryString["Item"].ToString()
                //                                , txtBfrTrnsROBFrom.Text
                //                                //, txtBfrTrnsROBTo.Text
                //                                , robTo
                //                                , (Convert.ToInt32(txtBfrTrnsROBFrom.Text) - Convert.ToInt32(txtAfrTrnsROBFrom.Text)).ToString()
                //                                , txtOperationDate.Text
                //                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                //                                , ref TranscationId
                //                            );

                //PhoenixElog.TankDescUpdate(
                //                    usercode
                //                 , ReportCode
                //                 , ddlTransferFrom.SelectedItem.Text
                //                 ,  TransferToSelectedText
                //                 , Request.QueryString["Item"].ToString()
                //                 , txtBfrTrnsROBFrom.Text
                //                 , robTo
                //                 , (Convert.ToInt32(txtBfrTrnsROBFrom.Text) - Convert.ToInt32(txtAfrTrnsROBFrom.Text)).ToString()
                //                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                //                 , TranscationId
                //                 , txtOtherInfo1.Text
                //                 , txtOtherInfo2.Text
                //             );

                if (ViewState["otherInfo"] != null && Convert.ToBoolean(ViewState["otherInfo"]) == true)
                {
                    //PhoenixElog.LogOtherDetailInsert(
                    //            TranscationId
                    //            , lblOtherInfo1.Text
                    //            , txtOtherInfo1.Text
                    //            , lblOtherInfo2.Text
                    //            , txtOtherInfo2.Text
                    //            , usercode
                    //    );
                }


                txtAfrTrnsROBTo.Text = ((Convert.ToInt32(txtBfrTrnsROBFrom.Text) - Convert.ToInt32(txtAfrTrnsROBFrom.Text)) + Convert.ToInt32(robTo)).ToString() ; // Convert.ToInt32(txtBfrTrnsROBTo.Text)).ToString();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void ddlFromROB()
    {
        DataSet ds1 = PhoenixElog.TankCurrentROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                               , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                               , new Guid(ddlTransferFrom.SelectedItem.Value)
                                                );

        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = ds1.Tables[0].Rows[0];

            if (dr1["FLDROB"].ToString() == null || dr1["FLDROB"].ToString() == "")
            {

                DataSet ds = PhoenixElog.VesselROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , ddlTransferFrom.SelectedItem.Value
                                                  );

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtBfrTrnsROBFrom.Text = dr["FLDROB"].ToString();
                }


            }
            else
            {
                txtBfrTrnsROBFrom.Text = dr1["FLDROB"].ToString();
            }
        }
        else
        {
            txtBfrTrnsROBFrom.Text = "";
        }


        DataSet ds2 = PhoenixElog.VesselROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            , ddlTransferFrom.SelectedItem.Value
                                             );

        if (ds2.Tables[1].Rows.Count > 0)
        {
            DataRow dr2 = ds2.Tables[1].Rows[0];
            ViewState["ItemNObyTank"] = dr2["FLDITEMNO"].ToString();
            ViewState["ItemNamebyTank"] = dr2["FLDITEMNAME"].ToString();
        }
    }
    public void ddlTOROB()
    {
        ddlTransferTo.Items.Clear();
        //ddlTransferTo.DataSource = PhoenixElog.ddlTrasnferToLocation(ReportCode);
        ddlTransferTo.DataSource = PhoenixElog.ElogLocationDropDown(null, null, "FROM", vesselId, usercode); 
        ddlTransferTo.DataTextField = "FLDNAME";
        ddlTransferTo.DataValueField = "FLDLOCATIONID";
        ddlTransferTo.DataBind();

        foreach (RadComboBoxItem item in ddlTransferTo.Items)
        {
            item.Selected = true;
            break;
        }

        if (ddlTransferTo.SelectedItem != null)
        {

            DataSet ds1 = PhoenixElog.TankCurrentROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                          , new Guid(ddlTransferTo.SelectedItem.Value)
                                           );

            if (ds1.Tables[0].Rows.Count > 0)
            {
                DataRow dr1 = ds1.Tables[0].Rows[0];

                if (dr1["FLDROB"].ToString() == null || dr1["FLDROB"].ToString() == "")
                {

                    DataSet ds = PhoenixElog.VesselROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , ddlTransferTo.SelectedItem.Value
                                                  );
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        txtBfrTrnsROBTo.Text = dr["FLDROB"].ToString();
                    }
                }
                else
                {
                    txtBfrTrnsROBTo.Text = dr1["FLDROB"].ToString();
                }
            }
            else
            {
                txtBfrTrnsROBTo.Text = "";
            }
        }
    }

    public void BindLocation()
    {
        ddlTransferFrom.DataSource = PhoenixElog.ElogLocationDropDown(null, null, "FROM", vesselId, usercode);
        ddlTransferFrom.DataBind();
        foreach (RadComboBoxItem item in ddlTransferFrom.Items)
        {
            item.Selected = true;
            break;
        }
    }
    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFromROB();
        ddlTOROB();
    }

    protected void ddlTransferTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = ddlTransferTo.SelectedValue.ToString();
        if (ddlTransferTo.SelectedItem != null)
        {

            DataSet ds1 = PhoenixElog.TankCurrentROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , new Guid(ddlTransferTo.SelectedItem.Value)
                                         );

            if (ds1.Tables[0].Rows.Count > 0)
            {
                DataRow dr1 = ds1.Tables[0].Rows[0];

                if (dr1["FLDROB"].ToString() == null || dr1["FLDROB"].ToString() == "")
                {

                    DataSet ds = PhoenixElog.VesselROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , ddlTransferTo.SelectedItem.Value
                                                  );
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        txtBfrTrnsROBTo.Text = dr["FLDROB"].ToString();
                    }
                }
                else
                {
                    txtBfrTrnsROBTo.Text = dr1["FLDROB"].ToString();
                }
            }
            else
            {
                txtBfrTrnsROBTo.Text = "";
            }
        }

        if (ddlTransferTo.SelectedItem.Text == "Evaporation" || ddlTransferTo.SelectedItem.Text == "Incineration")
        {
            lblAfrTrnsROBTo.Enabled = false;
            lblAfrTrnsROBTo.Enabled = false;
            txtBfrTrnsROBTo.Enabled = false;
        }
        else
        {
            txtBfrTrnsROBTo.Enabled = true;
        }
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtOperationDate.Text) == null)
            ucError.ErrorMessage = "Date is required";
        if (General.GetNullableDateTime(txtOperationDate.Text) > System.DateTime.Now)
            ucError.ErrorMessage = "Future Date is not acceptable";
        if (General.GetNullableInteger(txtBfrTrnsROBFrom.Text) <= General.GetNullableInteger(txtAfrTrnsROBFrom.Text))
            ucError.ErrorMessage = "After ROB is not greater than Before ROB";
        if (General.GetNullableInteger(txtBfrTrnsROBFrom.Text) == null)
            ucError.ErrorMessage = "Before From ROB is required";
        if (General.GetNullableInteger(txtBfrTrnsROBTo.Text) == null)
            ucError.ErrorMessage = "Before To ROB is required";
        if (General.GetNullableInteger(txtAfrTrnsROBFrom.Text) == null)
            ucError.ErrorMessage = "After From ROB is required";
        if (General.GetNullableInteger(txtAfrTrnsROBTo.Text) == null)
            ucError.ErrorMessage = "After To ROB is required";
        if (ddlTransferTo.SelectedItem.Text == null || ddlTransferTo.SelectedItem.Text == "--Select--")
            ucError.ErrorMessage = "Transfer to is required";

        return (!ucError.IsError);
    }

    protected void ddlQuick_DataBound(object sender, EventArgs e)
    {
        //ddlTransferFrom.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        //ddlTransferFrom.Items.Insert(0, "--Select--");
        //ddlTransferTo.Items.Insert(0, "--Select--");
        //ddlItem.Items.Insert(0, "--Select--");
    }

    protected void ddl_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }


}