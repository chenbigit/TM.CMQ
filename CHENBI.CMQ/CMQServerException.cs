using System;

namespace CHENBI.CMQ {

    public class CMQServerException : Exception {

        private int httpStatus = 200;
        private int errorCode = 0;
        private string errorMessage = "";
        private string requestId = "";

        public CMQServerException(int status) {
            this.httpStatus = status;
        }
        public CMQServerException(int errorCode, string errorMessage, string requestId) {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
            this.requestId = requestId;
        }


        public virtual int ErrorCode {
            get {
                return errorCode;
            }
        }

        public virtual string ErrorMessage {
            get {
                return errorMessage;
            }
        }

        public virtual string RequestId {
            get {
                return requestId;
            }
        }

        public override string ToString() {
            if (httpStatus != 200) {
                return "http status:" + httpStatus;
            } else {
                return "code:" + errorCode + ", message:" + errorMessage + ", requestId:" + requestId;
            }
        }
    }

}