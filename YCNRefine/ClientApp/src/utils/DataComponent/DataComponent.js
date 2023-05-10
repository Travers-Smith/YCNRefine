import React, { Fragment } from "react";
import { Spinner } from "react-bootstrap";
import CustomAlert from "../../components/CustomAlert/CustomAlert";

const DataComponent = ({ isLoading, isError, setIsError, children, name }) => (
    <Fragment>
        {
            isLoading ?
                <div style={{ textAlign: "center", padding: 10}}>
                    <Spinner 
                        animation="border" 
                        variant="primary" 
                    />
                </div>
            : isError ?
                <CustomAlert
                    alertTitle={isError.title ? isError.title : "Internal Error"} 
                    dismissible
                    variant="danger"
                    setShow={setIsError && setIsError}
                >
                    {isError.body ? 
                    
                        isError.body 

                    :
                        `  
                            Sorry we are unable to retrieve the data for the ${name}, 
      
                            please check your internet connection and then refresh the page.
                        
                        `
                
                    }
                </CustomAlert>
            :
                children
        }
    </Fragment>
);

export default DataComponent;