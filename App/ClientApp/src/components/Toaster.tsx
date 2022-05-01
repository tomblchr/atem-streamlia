import * as React from "react";
import * as ReactDOM from "react-dom";
import { ToastContainer, Toast } from "react-bootstrap";
//import { Toast as T } from "bootstrap";

interface IToasterProps {
    message: string;
    show?: boolean;
    error?: boolean;
}

const Toaster = ({message, show = true, error = false}: IToasterProps): React.ReactElement => {

    const [state, setState] = React.useState<boolean>(show);

    return (
        <ToastContainer position="top-center">
            <Toast animation={true} autohide={true} delay={5000} show={state} bg={error ? "danger" : "success"} onClose={() => setState(false)}>
                <Toast.Header closeButton={false}>
                    <div>streamlia</div>
                </Toast.Header>
                <Toast.Body>
                    {message}
                </Toast.Body>
            </Toast>
        </ToastContainer>
    );

}

export const ToastMessage = (m: string): void => {
    alert('TM');
    ReactDOM.createPortal(<Toaster message={m} error={true} />, document.body);
}

export default Toaster;