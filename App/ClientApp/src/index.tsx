import 'bootstrap/dist/css/bootstrap.css';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import unregister from './registerServiceWorker';

const defaultUrl: string = ""; 
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');
const root = createRoot(rootElement!); 

root.render(
  <BrowserRouter basename={baseUrl ?? defaultUrl}>
    <App />
  </BrowserRouter>);

unregister();

