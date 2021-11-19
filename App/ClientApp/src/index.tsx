import 'bootstrap/dist/css/bootstrap.css';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import unregister from './registerServiceWorker';

const defaultUrl: string = ""; 
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl ?? defaultUrl}>
    <App />
  </BrowserRouter>,
  rootElement);

unregister();

