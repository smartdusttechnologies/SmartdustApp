import { Counter } from "./ecomponents/Counter";
import { FetchData } from "./ecomponents/FetchData";
import { Home } from "./ecomponents/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  }
];

export default AppRoutes;
